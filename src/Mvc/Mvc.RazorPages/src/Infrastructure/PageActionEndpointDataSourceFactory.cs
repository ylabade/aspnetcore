// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure
{
    internal class PageActionEndpointDataSourceFactory
    {
        private readonly PageActionEndpointDataSourceIdProvider _dataSourceIdProvider;
        private readonly IActionDescriptorCollectionProvider _actions;
        private readonly ActionEndpointFactory _endpointFactory;
        private readonly OrderedEndpointsSequenceProvider _orderProvider;

        public PageActionEndpointDataSourceFactory(
            PageActionEndpointDataSourceIdProvider dataSourceIdProvider,
            IActionDescriptorCollectionProvider actions,
            ActionEndpointFactory endpointFactory,
            OrderedEndpointsSequenceProvider orderProvider)
        {
            _dataSourceIdProvider = dataSourceIdProvider;
            _actions = actions;
            _endpointFactory = endpointFactory;
            _orderProvider = orderProvider;
        }

        public PageActionEndpointDataSource Create()
        {
            return new PageActionEndpointDataSource(_dataSourceIdProvider, _actions, _endpointFactory, _orderProvider);
        }
    }
}
