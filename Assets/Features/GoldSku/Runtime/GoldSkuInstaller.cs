using Core;
using VContainer;
using VContainer.Unity;

namespace Features.GoldSku
{
    public class GoldSkuInstaller : StaticScopeBase
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ISkuHandlerInternal, ISkuHandler, GoldSkuHandler>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GoldSkuEntryPoint>();
            builder.Register<GoldSkuHandlerController>(Lifetime.Transient);
        }
    }

    public class GoldSkuHandler : ConsumableSkuHandler //TODO store name in registry separately
    {
        public override string Name => "Gold";
    }
}