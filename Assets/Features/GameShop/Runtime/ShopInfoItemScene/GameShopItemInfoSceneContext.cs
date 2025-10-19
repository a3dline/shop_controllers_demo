using Core;
using UnityEngine;

namespace Features.GameShop
{
    public class GameShopItemInfoSceneContext : SceneContextBase
    {
        [SerializeField]
        private ShopCardInfoView _view;
        
        public ShopCardInfoView View => _view;
    }
}
