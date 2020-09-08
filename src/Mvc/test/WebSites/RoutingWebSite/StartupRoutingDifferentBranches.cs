// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace RoutingWebSite
{
    public class StartupRoutingDifferentBranches
    {
        // Set up application services
        public void ConfigureServices(IServiceCollection services)
        {
            var pageRouteTransformerConvention = new PageRouteTransformerConvention(new SlugifyParameterTransformer());

            services
                .AddMvc(ConfigureMvcOptions)
                .AddNewtonsoftJson()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AddPageRoute("/PageRouteTransformer/PageWithConfiguredRoute", "/PageRouteTransformer/NewConventionRoute/{id?}");
                    options.Conventions.AddFolderRouteModelConvention("/PageRouteTransformer", model =>
                    {
                        pageRouteTransformerConvention.Apply(model);
                    });
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            ConfigureRoutingServices(services);

            services.AddScoped<TestResponseGenerator>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.Map("/branch", branch =>
            {
                branch.UseRouting();

                branch.UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                });
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run(c =>
            {
                return c.Response.WriteAsync("Hello from middleware after routing");
            });
        }

        protected virtual void ConfigureMvcOptions(MvcOptions options)
        {
            // Add route token transformer to one controller
            options.Conventions.Add(new ControllerRouteTokenTransformerConvention(
                typeof(ParameterTransformerController),
                new SlugifyParameterTransformer()));
        }

        protected virtual void ConfigureRoutingServices(IServiceCollection services)
        {
            services.AddRouting(options => options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer));
        }
    }
}
