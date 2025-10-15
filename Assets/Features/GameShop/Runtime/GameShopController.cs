using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Game.Shop;

namespace Features.GameShop
{
    public class GameShopController : ControllerBase
    {
        public GameShopController(IControllerFactory controllerFactory) : base(controllerFactory) { }
        protected override async UniTask AsyncFlow(CancellationToken flowToken)
        {
            await StartAndWait<GameShopSceneController>(flowToken);
        }
    }
}