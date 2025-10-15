using System.Threading;
using AUniTaskSemaphore;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.GameShop
{
    internal class GameShopCardController : ControllerBase
    {
        private readonly IGameShopService _shopService;

        public GameShopCardController(IControllerFactory controllerFactory,
                                      IGameShopService shopService) : base(controllerFactory)
        {
            _shopService = shopService;
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
            
            view.SetHeaderText(bundle.Title);
            return UniTask.WaitUntilCanceled(flowToken); 
        }

        private async UniTask PurchaseFlow(ShopCardView view,BundleData bundle, CancellationToken flowToken)
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