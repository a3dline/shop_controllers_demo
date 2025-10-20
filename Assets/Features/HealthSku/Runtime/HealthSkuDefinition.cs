using System;
using Core;
using VContainer;

namespace Features.HealthSku
{
    public class HealthSkuDefinition : ISkuDefinition
    {
        [Inject]
        public HealthSkuDefinition(ISkuHandler handler)
        {
            Handler = handler;
        }

        public HealthSkuDefinition() { }
        public string SkuId => "health_sku";
        public string DisplayName => "Health";
        public Type SkuValueType => typeof(int);
        public ISkuHandler Handler { get; }
    }
}