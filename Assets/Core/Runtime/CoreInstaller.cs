using Core.AssetProvider;
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
                   .Keyed(ISceneProvider.Type.BuildIn);
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Addressable);
            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Resources);
        }
    }
}