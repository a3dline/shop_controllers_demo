using System.Threading;
using Core.Controllers;
using Core.Game;
using Cysharp.Threading.Tasks;

namespace Core
{
    public class AppRootController : RootController
    {
        public AppRootController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask<ControllerEmptyResult> AsyncFlow(CancellationToken flowToken)
        {
            await StartAndWaitInScope<GameController, GameScope>(flowToken);
            return default;
        }
    }
}