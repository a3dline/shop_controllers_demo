using System.Threading;
using Core;
using Core.EventsBus;
using Cysharp.Threading.Tasks;
using GameEvents.Runtime;
using UnityEngine;

namespace Features.GameShop
{
    internal class GameShopCardController : ControllerBase
    {
        private readonly IGameShopService _shopService;
        private readonly IEventBus _eventBus;

        public GameShopCardController(IControllerFactory controllerFactory,
                                      IGameShopService shopService,
                                      IEventBus eventBus) : base(controllerFactory)
        {
            _shopService = shopService;
            _eventBus = eventBus;
        }

        protected override UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var cardContext = (ShopCardContext)context;
            var prefab = cardContext.Prefab;
            var parent = cardContext.Parent;
            var bundle = cardContext.Bundle;

            var instance = Object.Instantiate(prefab, parent);
            RegisterDisposable(instance.ToDisposable());

            var view = instance.GetComponent<ShopCardView>();

            view.PurchaseBtnWasClicked += () => PurchaseFlow(view, bundle, flowToken).Forget();
            view.InfoBtnWasClicked += () => StartAndWait<GameShopCardInfoSceneController>(flowToken).Forget();
            view.SetHeaderText(bundle.Title);
            view.EnableInfoBtn = cardContext.DisplayInfoBtn;
            
            PurchaseButtonEnabledFlow(view, bundle, flowToken).Forget();
            return UniTask.WaitUntilCanceled(flowToken);
        }
        
        private async UniTaskVoid PurchaseButtonEnabledFlow(ShopCardView view, BundleData bundle, CancellationToken flowToken)
        {
            using var canPurchaseItemProperty = _shopService.CanPurchaseItemProperty(bundle);
            await foreach (var value in canPurchaseItemProperty.WithCancellation(flowToken))
            {
                view.EnablePurchaseBtn = value;
            }
        }

        private async UniTask PurchaseFlow(ShopCardView view, BundleData bundle, CancellationToken flowToken)
        {
            var origin = view.PurchaseBtnText;
            view.PurchaseBtnText = "Processing...";
            view.EnablePurchaseBtn = false;
            await _shopService.PurchaseItemAsync(bundle, flowToken);
            view.PurchaseBtnText = origin;
            view.EnablePurchaseBtn = true;
        }
    }
}
