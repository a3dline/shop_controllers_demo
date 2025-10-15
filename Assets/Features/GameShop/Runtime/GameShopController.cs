using System.Threading;
using Core;
using Cysharp.Threading.Tasks;

namespace Features.GameShop
{
    public class GameShopController : ControllerBase
    {
        public GameShopController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            StartAndWait<PopulateGameShopBuildInDataController>(flowToken).Forget();
            await StartAndWait<GameShopSceneController>(flowToken);
        }
    }
}