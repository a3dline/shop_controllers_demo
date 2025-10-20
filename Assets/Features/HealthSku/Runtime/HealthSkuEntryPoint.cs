using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.HealthSku
{
    public class HealthSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly HealthSkuDefinition _definition;
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public HealthSkuEntryPoint(IControllerFactory controllerFactory,
                                   ISkuRegistrationService skuRegistrationService,
                                   ISkuHandlerInternal skuHandler,
                                   HealthSkuDefinition definition)
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
            _skuHandler.DefaultBalance = 100;
            _skuRegistrationService.Register(_definition);

            return StartAndWait<HealthSkuHandlerController>(_definition.SkuId, flowToken);
        }
    }
}