using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.PolSku
{
    public class PolSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly ISkuHandler _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public PolSkuEntryPoint(IControllerFactory controllerFactory,
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
                               SkuId = "pol_sku",
                               DisplayName = "Point of Lives",
                               Handler = _skuHandler
                           };
            _skuRegistrationService.Register(registry);

            return StartAndWait<PolSkuHandlerController>(flowToken);
        }
    }
}