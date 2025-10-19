using System;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.BackedClient
{
    public class FakeBackendClient : IBackendClient
    {
        public async UniTask<TResponse> PostAsync<TRequest, TResponse>(string endpoint,
                                                                       TRequest requestData,
                                                                       CancellationToken token)
        {
            await UniTask.Delay(1000, cancellationToken: token);
            token.ThrowIfCancellationRequested();

            return endpoint switch
                   {
                       "game-shop/purchase-bundle" => (TResponse)CreateBundleResponse(typeof(TResponse),
                        requestData,
                        string.Empty,
                        ResponseStatus.Success),
                       _ => throw new
                                NotImplementedException($"FakeBackendClient does not implement endpoint: {endpoint}")
                   };
        }

        private static object CreateBundleResponse(Type dtoType,
                                                   object requestData,
                                                   string error,
                                                   ResponseStatus status)
        {
            var bundleId = requestData.GetType().GetProperty("BundleId")?.GetValue(requestData);

            var instance = Activator.CreateInstance(dtoType)!;
            var flags = BindingFlags.Public | BindingFlags.Instance;

            dtoType.GetField("BundleId", flags)?.SetValue(instance, bundleId);
            dtoType.GetField("Error", flags)?.SetValue(instance, error);
            dtoType.GetField("Status", flags)?.SetValue(instance, status);

            return instance;
        }
    }
}