using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Features.GameShop;

namespace Game
{
    public class GameRootController : RootControllerBase
    {
        public GameRootController(IControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var gameContext = (GameContext)context;
            await this.StartAndWaitInScope<GameShopController, GameShopScope>(gameContext.GameShopContext, flowToken);
        }
    }
}