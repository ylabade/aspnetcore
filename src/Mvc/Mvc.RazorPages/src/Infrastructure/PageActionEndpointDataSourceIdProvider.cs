// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    public class PageActionEndpointDataSourceIdProvider
    {
        public int nextId = 0;

        internal int CreateId()
        {
            return Interlocked.Increment(ref nextId);
        }
    }
}
