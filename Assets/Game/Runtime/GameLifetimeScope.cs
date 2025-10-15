using Core;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            CoreInstaller.Install(builder);
            builder.RegisterEntryPoint<GameEntryPoint>();
            builder.Register<GameRootController>(Lifetime.Transient);
        }
    }
}