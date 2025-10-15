using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public static class VContainerControllerExtensions
    {
        public static UniTask StartAndWaitInScope<TController, TScope>(
            this ControllerBase controller,
            CancellationToken token)
            where TController : IController<ControllerEmptyResult>
            where TScope : IInstaller, new()
        {
            return controller.StartAndWaitInScope<TController, TScope>(null, token);
        }
        
        public static async UniTask StartAndWaitInScope<TController, TScope>(
            this ControllerBase controller,
            object context,
            CancellationToken token)
            where TController : IController<ControllerEmptyResult>
            where TScope : IInstaller, new()
        {
            using var scope = controller.Resolver.CreateScope(builder =>
            {
                var installer = new TScope();
                installer.Install(builder);
            });

            var controllerFactory = scope.Resolve<IControllerFactory>();
            await controller.StartAndWait<TController>(context, controllerFactory, token);
        }
    }
}