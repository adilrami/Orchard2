﻿using Microsoft.AspNet.Http;
using Orchard.Environment.Cache.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orchard.Environment.Cache.CacheContextProviders
{
    public class QueryCacheContextProvider : ICacheContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string QueryPrefix = "query:";

        public QueryCacheContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task PopulateContextEntriesAsync(IEnumerable<string> contexts, List<CacheContextEntry> entries)
        {
            if (contexts.Any(ctx => String.Equals(ctx, "query", StringComparison.OrdinalIgnoreCase)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var query = httpContext.Request.Query;
                var allKeys = query.Keys.OrderBy(x => x).ToArray();
                entries.AddRange(allKeys
                    .Select(x => new CacheContextEntry(
                        key: x.ToLowerInvariant(), 
                        value: query[x].ToString().ToLowerInvariant())
                    ));

                // If we track any query value, we don't need to look into specific ones
                return Task.CompletedTask;
            }

            foreach (var context in contexts.Where(ctx => ctx.StartsWith(QueryPrefix, StringComparison.OrdinalIgnoreCase)))
            {
                var key = context.Substring(QueryPrefix.Length);

                var httpContext = _httpContextAccessor.HttpContext;
                var query = httpContext.Request.Query;
                entries.Add(new CacheContextEntry(
                        key: key.ToLowerInvariant(),
                        value: query[key].ToString().ToLowerInvariant())
                    );
            }

            return Task.CompletedTask;
        }
    }
}
