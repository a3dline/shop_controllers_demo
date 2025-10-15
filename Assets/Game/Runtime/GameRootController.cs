using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Features.GameShop;

namespace Game
{
    public class GameRootController : RootControllerBase
    {
        public GameRootController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask AsyncFlow(CancellationToken flowToken)
        {
            await this.StartAndWaitInScope<GameShopController, GameShopScope>(flowToken);
        }
    }
}