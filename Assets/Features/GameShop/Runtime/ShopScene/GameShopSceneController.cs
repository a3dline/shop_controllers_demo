using System.Threading;
using Core;
using Core.AssetProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Features.GameShop
{
    internal class GameShopSceneController : SceneControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IGameShopService _shopService;

        public GameShopSceneController(IControllerFactory controllerFactory,
                                       [Key(ISceneProvider.Type.BuildIn)] ISceneProvider sceneProvider,
                                       [Key(IAssetProvider.Type.Addressable)] IAssetProvider assetProvider,
                                       IGameShopService shopService)
            : base(controllerFactory, sceneProvider)
        {
            _assetProvider = assetProvider;
            _shopService = shopService;
        }

        protected override string SceneName => "GameShop";
        protected override LoadSceneMode LoadSceneMode => LoadSceneMode.Additive;

        protected override async UniTask AsyncFlow(SceneContextBase sceneContext,
                                                   object context,
                                                   CancellationToken flowToken)
        {
            var gameShopSceneContext = (GameShopSceneContext)sceneContext;
            var gameShopContext = (GameShopContext)context;
            var shopView = gameShopSceneContext.ShopView;

            var getShopDataTask = _shopService.GetShopDataAsync(flowToken);
            var getCardPrefabTask = _assetProvider.LoadAsync<GameObject>(gameShopContext.CardPrefabAddress, flowToken);

            var (shopData, cardPrefabHolder) = await UniTask.WhenAll(getShopDataTask, getCardPrefabTask);
            using var _ = cardPrefabHolder;

            foreach (var bundle in shopData.Bundles)
            {
                var cardContext = new ShopCardContext
                                  {
                                      Bundle = bundle,
                                      Prefab = cardPrefabHolder.Asset,
                                      Parent = shopView.CardsContainer
                                  };
                StartAndWait<GameShopCardController>(cardContext, flowToken).Forget();
            }

            await UniTask.WaitUntilCanceled(flowToken);
        }
    }
}