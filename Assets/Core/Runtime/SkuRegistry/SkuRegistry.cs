namespace Core
{
    public struct SkuRegistry
    {
        public string SkuId;
        public string DisplayName;
        public ISkuHandler Handler;
    }
}