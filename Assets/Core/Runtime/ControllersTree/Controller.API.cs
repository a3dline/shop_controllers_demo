using System.Threading;
using Core.Controllers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.ControllersTree
{
    public abstract partial class Controller<TResult>
    {
        protected async UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
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

        protected UniTask StartAndWait<TController>(CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            return StartAndWait<TController>(_controllerFactory, token);
        }

        protected async UniTask StartAndWaitInScope<TController, TScope>(CancellationToken token)
            where TController : IController<ControllerEmptyResult>
            where TScope : IInstaller, new()
        {
            using var scope = _resolver.CreateScope(builder =>
            {
                var installer = new TScope();
                installer.Install(builder);
            });

            var controllerFactory = scope.Resolve<IControllerFactory>();
            await StartAndWait<TController>(controllerFactory, token);
        }
    }
}