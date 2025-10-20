using System;
using Core;
using VContainer;

namespace Features.VipSku
{
    public class VipSkuDefinition : ISkuDefinition
    {
        [Inject]
        public VipSkuDefinition(ISkuHandler handler)
        {
            Handler = handler;
        }

        public VipSkuDefinition() { }
        public string SkuId => "vip_sku";
        public string DisplayName => "VIP";
        public ISkuHandler Handler { get; }
    }
}