using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class RootControllerBase : ControllerBase
    {
        public RootControllerBase(IControllerFactory controllerFactory) : base(controllerFactory) { }

        public void LaunchTree(CancellationToken flowToken, Action<Exception> exceptionHandler = null)
        {
            LaunchFlow(null, flowToken).Forget(exceptionHandler);
        }
        
        public void LaunchTree(object context, CancellationToken token, Action<Exception> exceptionHandler = null)
        {
            LaunchFlow(context, token).Forget(exceptionHandler);
        }
        
        private async UniTask LaunchFlow(object context, CancellationToken token)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationRegistry = cts.Token.Register(OnStop);
            try
            {
                await AsyncFlow(context, cts.Token);
            }
            finally
            {
                cancellationRegistry.Dispose();

                if (!cts.IsCancellationRequested)
                {
                    cts.Cancel();
                    OnStop();
                }
            }
        }
    }
}