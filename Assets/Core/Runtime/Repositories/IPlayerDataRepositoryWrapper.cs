using System;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IPlayerDataRepositoryWrapper
    {
        UniTask UpdateSku(string skuId, IConvertible data);
        UniTask<IConvertible> GetSkuData(string skuId);
    }
}