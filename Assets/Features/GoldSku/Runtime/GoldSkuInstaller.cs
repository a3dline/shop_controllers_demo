using Core;
using VContainer;
using VContainer.Unity;

namespace Features.GoldSku
{
    public class GoldSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, ConsumableSkuHandler>(Lifetime.Singleton);
            builder.Register<GoldSkuDefinition>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GoldSkuEntryPoint>();
            builder.Register<GoldSkuHandlerController>(Lifetime.Transient);
        }
    }
}