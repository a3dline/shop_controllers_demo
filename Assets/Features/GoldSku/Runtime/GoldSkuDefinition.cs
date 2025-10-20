using System;
using Core;
using VContainer;

namespace Features.GoldSku
{
    internal class GoldSkuDefinition : ISkuDefinition
    {
        [Inject]
        public GoldSkuDefinition(ISkuHandler handler)
        {
            Handler = handler;
        }

        public GoldSkuDefinition() { }
        public string SkuId => "gold_sku";
        public string DisplayName => "Gold";
        public ISkuHandler Handler { get; }
    }
}