using System.Threading;
using AControllersTree;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.LocationSku
{
    internal class LocationSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly LocationSkuDefinition _definition;
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public LocationSkuEntryPoint(IControllerFactory controllerFactory,
                                     ISkuRegistrationService skuRegistrationService,
                                     ISkuHandlerInternal skuHandler,
                                     LocationSkuDefinition definition)
            : base(controllerFactory)
        {
            _skuRegistrationService = skuRegistrationService;
            _skuHandler = skuHandler;
            _definition = definition;
        }

        public UniTask StartAsync(CancellationToken cancellation = default)
        {
            LaunchTree(cancellation);
            return UniTask.WaitUntilCanceled(cancellation);
        }

        protected override UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            _skuHandler.DefaultBalance = "default";
            _skuRegistrationService.Register(_definition);

            return StartAndWait<LocationSkuHandlerController>(_definition.SkuId, flowToken);
        }
    }
}