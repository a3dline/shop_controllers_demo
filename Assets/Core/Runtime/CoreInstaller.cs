using VContainer;

namespace Core
{
    public static class CoreInstaller
    {
        public static void Install(IContainerBuilder builder)
        {
            builder.Register<IControllerFactory, VContainerControllerFactory>(Lifetime.Scoped);

            // Services
            builder.Register<ISceneProvider, BuildInSceneProvider>(Lifetime.Singleton)
                   .Keyed(SceneProviderType.BuildIn);
        }
    }
}