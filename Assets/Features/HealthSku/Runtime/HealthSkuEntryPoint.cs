using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.HealthSku
{
    public class HealthSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly ISkuHandler _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public HealthSkuEntryPoint(IControllerFactory controllerFactory,
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
                               SkuId = "health_sku",
                               DisplayName = "Health", // TO to context
                               Handler = _skuHandler
                           };
            _skuRegistrationService.Register(registry);

            return StartAndWait<HealthSkuHandlerController>(flowToken);
        }
    }
}