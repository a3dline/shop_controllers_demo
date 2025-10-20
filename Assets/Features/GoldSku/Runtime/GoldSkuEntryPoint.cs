using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.GoldSku
{
    internal class GoldSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly GoldSkuDefinition _definition;
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public GoldSkuEntryPoint(IControllerFactory controllerFactory,
                                 ISkuRegistrationService skuRegistrationService,
                                 ISkuHandlerInternal skuHandler,
                                 GoldSkuDefinition definition)
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
            _skuHandler.DefaultBalance = 10;
            _skuRegistrationService.Register(_definition);

            return StartAndWait<GoldSkuHandlerController>(_definition.SkuId, flowToken);
        }
    }
}