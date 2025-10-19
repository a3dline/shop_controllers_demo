using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal class SkuRegistrationService : ISkuRegistrationService
    {
        private readonly Dictionary<string, string> _skuDisplayNames = new();
        private readonly Dictionary<string, ISkuHandler> _skuHandlers = new();

        public void Register(in SkuRegistry registry)
        {
            if (!_skuHandlers.TryAdd(registry.SkuId, registry.Handler))
            {
                Debug.LogError("SKU already registered: " + registry.SkuId);
                return;
            }

            registry.Handler.SkuId = registry.SkuId;
            _skuDisplayNames.Add(registry.SkuId, registry.DisplayName);
        }

        public IEnumerable<ISkuHandler> SkuHandlers => _skuHandlers.Values;

        public ISkuHandler GetSkuHandler(string skuId)
        {
            return _skuHandlers.GetValueOrDefault(skuId);
        }

        public string GetSkuDisplayName(string skuId)
        {
            return _skuDisplayNames.TryGetValue(skuId, out var displayName) ? displayName : string.Empty;
        }
    }
}