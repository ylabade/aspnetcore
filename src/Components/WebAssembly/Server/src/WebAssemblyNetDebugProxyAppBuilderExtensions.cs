// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Provides infrastructure for debugging Blazor WebAssembly applications.
    /// </summary>
    public static class WebAssemblyNetDebugProxyAppBuilderExtensions
    {
        /// <summary>
        /// Adds middleware needed for debugging Blazor WebAssembly applications
        /// inside Chromium dev tools.
        /// </summary>
        public static void UseWebAssemblyDebugging(this IApplicationBuilder app)
        {
            app.Map("/_framework/is-debugging", app =>
            {
                app.Run(async (context) =>
                {
                    var isDebugging = IsRunningDebuggableBrowser();
                    await context.Response.WriteAsync(isDebugging.ToString());
                });

            });
            app.Map("/_framework/debug", app =>
            {
                app.Use(async (context, next) =>
                {
                    var debugProxyBaseUrl = await DebugProxyLauncher.EnsureLaunchedAndGetUrl(context.RequestServices);
                    var requestPath = context.Request.Path.ToString();
                    if (requestPath == string.Empty)
                    {
                        requestPath = "/";
                    }

                    // Although we could redirect for every URL we see here, we filter the allowed set
                    // to ensure this doesn't get misused as some kind of more general redirector
                    switch (requestPath)
                    {
                        case "/":
                        case "/ws-proxy":
                            context.Response.Redirect($"{debugProxyBaseUrl}{requestPath}{context.Request.QueryString}");
                            break;
                        default:
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                    }
                });
            });
        }

        private static Boolean IsRunningDebuggableBrowser()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE CommandLine Like '%remote-debugging-port%'");
                var objects = searcher.Get();
                return objects.Count > 0;
            }
            else
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"ps aux | grep -v grep | grep -q 'remote-debugging-port='\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                process.WaitForExit();

                // grep returns a 0 exit code if a match was found
                return process.ExitCode == 0;
            }
        }
    }
}
