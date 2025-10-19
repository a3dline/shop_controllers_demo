using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    internal class PlayerDataRepositoryWrapper : IPlayerDataRepositoryWrapper
    {
        private const string SKUPrefix = "sku_";

        private readonly IRepository _repository;
        private Dictionary<string, AsyncReactiveProperty<IConvertible>> _skuProperties = new();

        public PlayerDataRepositoryWrapper(IRepository repository)
        {
            _repository = repository;
        }

        public async UniTask UpdateSku(string skuId, IConvertible data, CancellationToken token)
        {
            await _repository.UpsetAsync(SKUPrefix + skuId, data.ToString(CultureInfo.InvariantCulture), token);
            var property = GetOrCreateSkuProperty(skuId, data);
            property.Value = data;
        }

        public async UniTask<IReadOnlyAsyncReactiveProperty<IConvertible>> GetSkuPropertyAsync(string skuId, CancellationToken token)
        {
            var value = await _repository.GetAsync(SKUPrefix + skuId, token);
            var property = GetOrCreateSkuProperty(skuId, value);
            property.Value = value;
            return property;
        }

        private AsyncReactiveProperty<IConvertible> GetOrCreateSkuProperty(string skuId, IConvertible value)
        {
            if (!_skuProperties.TryGetValue(skuId, out var property))
            {
                property = new AsyncReactiveProperty<IConvertible>(value);
                _skuProperties[skuId] = property;
            }

            return property;
        }
    }
}