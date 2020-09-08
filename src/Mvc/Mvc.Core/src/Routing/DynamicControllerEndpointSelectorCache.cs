// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    internal class DynamicControllerEndpointSelectorCache : IDisposable
    {
        private ConcurrentDictionary<int, EndpointDataSource> _dataSourceCache = new();
        private ConcurrentDictionary<int, DynamicControllerEndpointSelector> _endpointSelectorCache = new();

        public void Dispose()
        {
            _dataSourceCache.Clear();
            _endpointSelectorCache.Clear();
        }

        public void AddDataSource(ControllerActionEndpointDataSource dataSource)
        {
            _dataSourceCache.GetOrAdd(dataSource.DataSourceId, dataSource);
        }

        // For testing purposes only
        internal void AddDataSource(EndpointDataSource dataSource, int key) =>
            _dataSourceCache.GetOrAdd(key, dataSource);

        public DynamicControllerEndpointSelector GetEndpointSelector(Endpoint endpoint)
        {
            if (endpoint?.Metadata == null)
            {
                return null;
            }

            var dataSourceId = endpoint.Metadata.GetMetadata<EndpointDataSourceIdMetadata>();
            return _endpointSelectorCache.GetOrAdd(dataSourceId.Id, (int key) => EnsureDataSource(key));

        }
        private DynamicControllerEndpointSelector EnsureDataSource(int key)
        {
            if (!_dataSourceCache.TryGetValue(key, out var dataSource))
            {
                throw new InvalidOperationException($"Data source with key '{key}' not registered.");
            }

            return new DynamicControllerEndpointSelector(dataSource);
        }
    }

    internal record EndpointDataSourceIdMetadata(int Id);
}
