using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract partial class ControllerBase<TResult>
    {
        public async UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
            CancellationToken token)
            where TController : IController<TControllerResult>
        {
            var child = _controllerFactory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationRegistry = cts.Token.Register(() => { child.StopSelf(); });
            TControllerResult result;

            try
            {
                result = await child.RunAsyncFlow(cts.Token);
            }
            finally
            {
                cancellationRegistry.Dispose();

                if (!cts.IsCancellationRequested)
                {
                    cts.Cancel();
                    child.StopSelf();
                }
            }

            return result;
        }

        public UniTask StartAndWait<TController>(CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            return StartAndWait<TController>(_controllerFactory, token);
        }

        
    }
}