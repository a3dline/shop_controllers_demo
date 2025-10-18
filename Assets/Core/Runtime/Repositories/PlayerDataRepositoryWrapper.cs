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

        public UniTask UpdateSku(string skuId, string data)
        {           
            return _repository.UpsetAsync(SKUPrefix + skuId, data);
            
        }
        public UniTask<string> GetSkuData(string skuId)
        {
            return _repository.GetAsync(SKUPrefix + skuId);
        }
    }
}