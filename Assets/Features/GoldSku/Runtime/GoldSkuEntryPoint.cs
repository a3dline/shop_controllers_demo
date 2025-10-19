using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.GoldSku
{
    internal class GoldSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private const string SkuId = "gold_sku";
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public GoldSkuEntryPoint(IControllerFactory controllerFactory,
                                 ISkuRegistrationService skuRegistrationService,
                                 ISkuHandlerInternal skuHandler)
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
                               DisplayName = "Gold",
                               Handler = _skuHandler
                           };
            _skuHandler.DefaultBalance = 10;
            _skuRegistrationService.Register(registry);

            return StartAndWait<GoldSkuHandlerController>(SkuId, flowToken);
        }
    }
}