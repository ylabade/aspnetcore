// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Routing;

namespace Microsoft.AspNetCore.Mvc.Infrastructure
{
    internal class ControllerActionEndpointDataSourceFactory
    {
        private readonly OrderedEndpointsSequenceProvider _orderProvider;
        private readonly ControllerActionEndpointDataSourceIdProvider _dataSourceIdProvider;
        private readonly IActionDescriptorCollectionProvider _actions;
        private readonly ActionEndpointFactory _factory;

        public ControllerActionEndpointDataSourceFactory(
            ControllerActionEndpointDataSourceIdProvider dataSourceIdProvider,
            IActionDescriptorCollectionProvider actions,
            ActionEndpointFactory factory,
            OrderedEndpointsSequenceProvider orderProvider)
        {
            _orderProvider = orderProvider;
            _dataSourceIdProvider = dataSourceIdProvider;
            _actions = actions;
            _factory = factory;
        }

        public ControllerActionEndpointDataSource Create()
        {
            return new ControllerActionEndpointDataSource(_dataSourceIdProvider, _actions, _factory, _orderProvider);
        }
    }
}
