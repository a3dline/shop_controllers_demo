using Core;
using VContainer;
using VContainer.Unity;

namespace Features.VipSku
{
    public class VipSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, ConsumableTimeSpanSkuHandler>(Lifetime.Singleton);
            builder.RegisterEntryPoint<VipSkuEntryPoint>();
            builder.Register<VipSkuHandlerController>(Lifetime.Transient);
        }
    }
}