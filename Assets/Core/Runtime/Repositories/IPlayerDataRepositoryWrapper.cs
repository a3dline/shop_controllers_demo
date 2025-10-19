using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IPlayerDataRepositoryWrapper
    {
        UniTask UpdateSku(string skuId, IConvertible data, CancellationToken token);
        UniTask<IReadOnlyAsyncReactiveProperty<IConvertible>> GetSkuPropertyAsync(string skuId, CancellationToken token);
    }
}