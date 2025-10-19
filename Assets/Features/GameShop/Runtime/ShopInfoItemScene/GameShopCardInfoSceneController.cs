using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace Features.GameShop
{
    public class GameShopCardInfoSceneController : SceneControllerBase
    {
        public GameShopCardInfoSceneController(IControllerFactory controllerFactory,
                                               [Key(ISceneProvider.Type.BuildIn)] ISceneProvider sceneProvider) :
            base(controllerFactory, sceneProvider) { }

        protected override UniTask AsyncFlow(SceneContextBase sceneContext, object context, CancellationToken flowToken)
        {
            var exitTaskSource = new UniTaskCompletionSource();
            var cardInfoSceneContext = (GameShopItemInfoSceneContext)sceneContext;
            var shopContext = (ShopCardContext)context;

            var view = cardInfoSceneContext.View;
            view.ExitButtonWasClicked += () => exitTaskSource.TrySetResult();

            var ctx = shopContext with { Parent = view.CardContainer, DisplayInfoBtn = false };
            StartAndWait<GameShopCardController>(ctx, flowToken).Forget();

            return exitTaskSource.Task;
        }

        protected override (string, LoadSceneMode) GetSceneNameAndMode(object context)
        {
            var shopContext = (ShopCardContext)context;
            return (shopContext.ShopCardInfoSceneName, LoadSceneMode.Additive);
        }
    }
}