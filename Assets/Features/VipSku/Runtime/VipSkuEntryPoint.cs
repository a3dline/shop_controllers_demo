using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.VipSku
{
    public class VipSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly VipSkuDefinition _definition;
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public VipSkuEntryPoint(IControllerFactory controllerFactory,
                                ISkuRegistrationService skuRegistrationService,
                                ISkuHandlerInternal skuHandler,
                                VipSkuDefinition definition)
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
            _skuHandler.DefaultBalance = 60;
            _skuRegistrationService.Register(_definition);

            return StartAndWait<VipSkuHandlerController>(_definition.SkuId, flowToken);
        }
    }
}