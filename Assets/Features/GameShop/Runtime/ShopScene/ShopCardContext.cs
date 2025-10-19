using UnityEngine;

namespace Features.GameShop
{
    internal record ShopCardContext
    {
        public BundleData Bundle;
        public GameObject Prefab;
        public Transform Parent;
        public string ShopCardInfoSceneName;
        public bool DisplayInfoBtn;
    }
}