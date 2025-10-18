using Core;
using Features.GoldSku;
using VContainer;
using VContainer.Unity;

namespace Features.PolSku
{
    public class PolSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, ConsumableSkuHandler>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PolSkuEntryPoint>();
            builder.Register<PolSkuHandlerController>(Lifetime.Transient);
        }
    }
}