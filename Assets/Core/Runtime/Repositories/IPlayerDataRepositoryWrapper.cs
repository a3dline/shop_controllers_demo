using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IPlayerDataRepositoryWrapper
    {
        UniTask UpdateSku(string skuId, string data);
        UniTask<string> GetSkuData(string skuId);
    }
}