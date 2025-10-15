using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace Features.GameShop
{
    internal class GameShopSceneController : SceneControllerBase
    {
        public GameShopSceneController(IControllerFactory controllerFactory,
                                       [Key(ISceneProvider.Type.BuildIn)] ISceneProvider sceneProvider) :
            base(controllerFactory, sceneProvider) { }

        protected override string SceneName => "GameShop";
        protected override LoadSceneMode LoadSceneMode => LoadSceneMode.Additive;

        protected override UniTask AsyncFlow(SceneContextBase sceneContext, object context, CancellationToken flowToken)
        {
            var gameShopSceneContext = sceneContext as GameShopSceneContext;
            return UniTask.WaitUntilCanceled(flowToken);
        }
    }
}