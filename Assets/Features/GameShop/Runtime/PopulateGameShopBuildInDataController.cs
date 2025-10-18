using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Features.GameShop
{
    public class PopulateGameShopBuildInDataController : ControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private readonly string _gameShopDataAddress;
        private readonly IGameShopService _shopService;

        public PopulateGameShopBuildInDataController(IControllerFactory controllerFactory,
                                                     IGameShopService shopService,
                                                     [Key(IAssetProvider.Type.Resources)]
                                                     IAssetProvider assetProvider) : base(controllerFactory)
        {
            _shopService = shopService;
            _assetProvider = assetProvider;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var shopContext = (GameShopContext)context;
            using var dataHodler =
                await _assetProvider.LoadAsync<ScriptableGameShopData>(shopContext.BuildInDataResourcesAddress, flowToken);
            var data = dataHodler.Asset.Data;

            //TODO validate data

            _shopService.UpdateData(data);
        }
    }
}