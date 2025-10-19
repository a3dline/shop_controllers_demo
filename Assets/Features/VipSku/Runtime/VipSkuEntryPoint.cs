using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Features.VipSku
{
    public class VipSkuEntryPoint : RootControllerBase, IAsyncStartable
    {
        private const string SkuId = "vip_sku";
        private readonly ISkuHandlerInternal _skuHandler;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public VipSkuEntryPoint(IControllerFactory controllerFactory,
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
                               DisplayName = "VIP", 
                               Handler = _skuHandler
                           };
            _skuHandler.DefaultBalance = 60;
            _skuRegistrationService.Register(registry);

            return StartAndWait<VipSkuHandlerController>(SkuId, flowToken);
        }
    }
}