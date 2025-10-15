using System;

namespace Features.GameShop
{
    [Serializable]
    public struct SkuData
    {
        public string SkuId;
        public int Amount;
    }

    [Serializable]
    public struct BundleData
    {
        public SkuData[] ConsumableData;
        public SkuData[] RewardData;
        public string Title;
    }

    [Serializable]
    public struct GameShopData
    {
        public BundleData[] Bundles;
    }
}