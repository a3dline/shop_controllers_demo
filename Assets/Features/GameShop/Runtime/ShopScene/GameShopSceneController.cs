using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.Shop
{
    internal class GameShopSceneController : SceneControllerBase
    {
        public GameShopSceneController(IControllerFactory controllerFactory,
                                       [Key(SceneProviderType.BuildIn)]ISceneProvider sceneProvider) :
            base(controllerFactory, sceneProvider) { }

        protected override string SceneName => "GameShop";
        protected override LoadSceneMode LoadSceneMode => LoadSceneMode.Additive;

        protected override UniTask AsyncFlow(SceneContextBase context, CancellationToken flowToken)
        {
            var shopContext = context as GameShopSceneContext;
            return UniTask.WaitUntilCanceled(flowToken);
        }
    }
}