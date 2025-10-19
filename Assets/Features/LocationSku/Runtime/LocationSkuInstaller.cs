using Core;
using VContainer;
using VContainer.Unity;

namespace Features.LocationSku
{
    public class LocationSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, FixedSkuHandler>(Lifetime.Singleton);
            builder.RegisterEntryPoint<LocationSkuEntryPoint>();
            builder.Register<LocationSkuHandlerController>(Lifetime.Transient);
        }
    }
}