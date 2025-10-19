using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarItemController : ControllerBase
    {
        private readonly ISkuRegistrationService _skuRegistrationService;

        public BalanceBarItemController(IControllerFactory controllerFactory,
                                        ISkuRegistrationService skuRegistrationService) : base(controllerFactory)
        {
            _skuRegistrationService = skuRegistrationService;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var itemContext = (BalanceBarItemContext)context;
            var prefab = itemContext.Prefab;
            var parent = itemContext.Parent;
            var skuHandler = itemContext.SkuHandler;

            var instance = Object.Instantiate(prefab, parent);
            using var _ = instance.ToDisposable();

            var view = instance.GetComponent<BalanceBarItemView>();
            var displayName = _skuRegistrationService.GetSkuDisplayName(skuHandler.SkuId);
            view.SetLabel(displayName);
            
            await foreach (var value in skuHandler.BalanceString.WithCancellation(flowToken))
            {
                view.SetBalance(value);
            }
        }
    }
}