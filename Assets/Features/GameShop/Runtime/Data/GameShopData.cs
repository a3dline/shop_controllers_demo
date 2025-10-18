using System;

namespace Features.GameShop
{
    [Serializable]
    public struct SkuData
    {
        public string SkuId;
        public string Amount;
    }

    [Serializable]
    public struct BundleData
    {
        public SkuData[] PurchaseData;
        public SkuData[] RewardData;
        public string Title;
        public string Id;
    }

    [Serializable]
    public struct GameShopData
    {
        public BundleData[] Bundles;
    }
}