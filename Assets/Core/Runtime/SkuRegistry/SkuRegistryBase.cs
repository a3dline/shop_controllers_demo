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

            registry.Handler.Id = registry.SkuId;
            _skuDisplayNames.Add(registry.SkuId, registry.DisplayName);
        }

        public IEnumerable<ISkuHandler> SkuHandlers => _skuHandlers.Values;

        public string GetSkuDisplayName(string skuHandlerId)
        {
            return _skuDisplayNames.TryGetValue(skuHandlerId, out var displayName) ? displayName : string.Empty;
        }
    }
}