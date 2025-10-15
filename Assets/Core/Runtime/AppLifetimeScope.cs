using Core.Controllers;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class AppLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AppEntryPoint>(); 
            builder.Register<AppRootController>(Lifetime.Transient);
            builder.Register<IControllerFactory, VContainerControllerFactory>(Lifetime.Scoped);
        }
    }
}