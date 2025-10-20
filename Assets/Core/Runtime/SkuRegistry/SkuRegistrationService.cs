using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    internal class SkuRegistrationService : ISkuRegistrationService
    {
        private readonly Dictionary<string, string> _skuDisplayNames = new();
        private readonly Dictionary<string, ISkuHandler> _skuHandlers = new();

        public void Register(ISkuDefinition definition)
        {
            if (!_skuHandlers.TryAdd(definition.SkuId, definition.Handler))
            {
                Debug.LogError("SKU already registered: " + definition.SkuId);
                return;
            }

            definition.Handler.SkuId = definition.SkuId;
            _skuDisplayNames.Add(definition.SkuId, definition.DisplayName);
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