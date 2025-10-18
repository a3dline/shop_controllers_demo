using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal class SkuRegistrationService : ISkuRegistrationService
    {
        private readonly Dictionary<string, ISkuHandler> _skuHandlers = new();
        public void Register(in SkuRegistry registry)
        {
            if (!_skuHandlers.TryAdd(registry.SkuId, registry.Handler))
            {
                Debug.LogError("SKU already registered: " + registry.SkuId);
            }
        }

        public IEnumerable<ISkuHandler> SkuHandlers => _skuHandlers.Values; 
    }
}