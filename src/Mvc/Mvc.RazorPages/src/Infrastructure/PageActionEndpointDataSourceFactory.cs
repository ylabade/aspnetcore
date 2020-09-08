// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    internal class PageActionEndpointDataSourceFactory
    {
        private readonly IActionDescriptorCollectionProvider _actions;
        private readonly ActionEndpointFactory _endpointFactory;

        public PageActionEndpointDataSourceFactory(IActionDescriptorCollectionProvider actions, ActionEndpointFactory endpointFactory)
        {
            _actions = actions;
            _endpointFactory = endpointFactory;
        }

        public PageActionEndpointDataSource Create()
        {
            return new PageActionEndpointDataSource(_actions, _endpointFactory);
        }
    }
}
