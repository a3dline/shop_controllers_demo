using Core.BackedClient;
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
            builder.Register<ISceneProvider, BuildInSceneProvider>(Lifetime.Singleton);
            
            builder.Register<IAssetProvider, AddressableAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Addressable);

            builder.Register<IAssetProvider, ResourcesAssetProvider>(Lifetime.Singleton)
                   .Keyed(IAssetProvider.Type.Resources);

            builder.Register<ISkuRegistrationService, SkuRegistrationService>(Lifetime.Singleton);
            builder.Register<IRepository, PlayerPrefsRepository>(Lifetime.Singleton);
            builder.Register<IPlayerDataRepository, PlayerDataRepositoryWrapper>(Lifetime.Singleton);
            builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
            builder.Register<IBackendClient, FakeBackendClient>(Lifetime.Singleton);
        }
    }
}