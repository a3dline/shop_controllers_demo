using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.BackedClient
{
    public interface IBackendClient
    {
        UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint,
                                                          TRequest requestData,
                                                          CancellationToken token);
    }
}