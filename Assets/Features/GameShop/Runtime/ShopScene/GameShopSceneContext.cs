using Core;
using UnityEngine;

namespace Features.GameShop
{
    internal class GameShopSceneContext : SceneContextBase
    {
        [SerializeField]
        private ShopView _shopView;
        
        [SerializeField]
        private Transform _balanceBarParent;

        public ShopView ShopView => _shopView;
        public Transform BalanceBarParent => _balanceBarParent;
    }
}