using System;
using System.Globalization;
using Cysharp.Threading.Tasks;

namespace Core
{
    internal class PlayerDataRepositoryWrapper : IPlayerDataRepositoryWrapper
    {
        private const string SKUPrefix = "sku_";

        private readonly IRepository _repository;

        public PlayerDataRepositoryWrapper(IRepository repository)
        {
            _repository = repository;
        }

        public UniTask UpdateSku(string skuId, IConvertible data)
        {
            return _repository.UpsetAsync(SKUPrefix + skuId, data.ToString(CultureInfo.InvariantCulture));
        }

        public async UniTask<IConvertible> GetSkuData(string skuId)
        {
            var value = await _repository.GetAsync(SKUPrefix + skuId);
            return value;
        }
    }
}