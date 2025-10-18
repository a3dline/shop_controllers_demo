using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarItemController : ControllerBase
    {
        public BalanceBarItemController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var itemContext = (BalanceBarItemContext)context;
            var prefab = itemContext.Prefab;
            var parent = itemContext.Parent;
            var skuHandler = itemContext.SkuHandler;

            var instance = Object.Instantiate(prefab, parent);
            using var _ = instance.ToDisposable();

            var view = instance.GetComponent<BalanceBarItemView>();
            view.SetLabel(skuHandler.Name);
            
            await foreach (var value in skuHandler.BalanceStringProperty.WithCancellation(flowToken))
            {
                view.SetBalance(value);
            }
        }
    }
}