using Core;
using UnityEngine;

namespace Features.GameShop
{
    internal class GameShopSceneContext : SceneContextBase
    {
        [SerializeField]
        private ShopView _shopView;

        public ShopView ShopView => _shopView;
    }
}