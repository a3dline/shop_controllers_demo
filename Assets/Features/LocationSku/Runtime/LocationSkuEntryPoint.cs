using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.LocationSku
{
    internal class LocationSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private const string SkuId = "location_sku";
        private readonly ISkuHandler _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public LocationSkuEntryPoint(IControllerFactory controllerFactory,
                                     ISkuRegistrationService skuRegistrationService,
                                     ISkuHandler skuHandler)
            : base(controllerFactory)
        {
            _skuRegistrationService = skuRegistrationService;
            _skuHandler = skuHandler;
        }

        public UniTask StartAsync(CancellationToken cancellation = default)
        {
            LaunchTree(cancellation);
            return UniTask.WaitUntilCanceled(cancellation);
        }

        protected override UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var registry = new SkuRegistry
                           {
                               SkuId = SkuId,
                               DisplayName = "Location",
                               Handler = _skuHandler
                           };
            _skuRegistrationService.Register(registry);

            return StartAndWait<LocationSkuHandlerController>(SkuId, flowToken);
        }
    }
}