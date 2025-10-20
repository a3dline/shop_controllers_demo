using System;
using Core;
using VContainer;

namespace Features.LocationSku
{
    public class LocationSkuDefinition : ISkuDefinition
    {
        [Inject]
        public LocationSkuDefinition(ISkuHandler handler)
        {
            Handler = handler;
        }

        public LocationSkuDefinition() { }
        public string SkuId => "location_sku";
        public string DisplayName => "Location";
        public ISkuHandler Handler { get;  }
    }
}