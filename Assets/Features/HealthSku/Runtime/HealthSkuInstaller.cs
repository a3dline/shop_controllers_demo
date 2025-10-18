using Core;
using VContainer;
using VContainer.Unity;

namespace Features.HealthSku
{
    public class HealthSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, ConsumableSkuHandler>(Lifetime.Singleton);
            builder.RegisterEntryPoint<HealthSkuEntryPoint>();
            builder.Register<HealthSkuHandlerController>(Lifetime.Transient);
        }
    }
}