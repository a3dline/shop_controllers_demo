using Core.EventsBus;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class CoreInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<IControllerFactory, VContainerControllerFactory>(Lifetime.Scoped);

            // Services
            builder.Register<ISceneProvider, BuildInSceneProvider>(Lifetime.Singleton)
                   .Keyed(ISceneProvider.Type.BuildIn);
            
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Addressable);

            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Resources);

            builder.Register<ISkuRegistrationService, SkuRegistrationService>(Lifetime.Singleton);
            builder.Register<IRepository, InMemoryRepository>(Lifetime.Singleton);
            builder.Register<IPlayerDataRepositoryWrapper, PlayerDataRepositoryWrapper>(Lifetime.Singleton);
            builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
        }
    }
}