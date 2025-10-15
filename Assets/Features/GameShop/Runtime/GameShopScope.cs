using VContainer;
using VContainer.Unity;

namespace Features.GameShop
{
    public class GameShopScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<GameShopController>(Lifetime.Transient);
            builder.Register<GameShopSceneController>(Lifetime.Transient);
            builder.Register<GameShopCardController>(Lifetime.Transient);
            builder.Register<PopulateGameShopBuildInDataController>(Lifetime.Transient);

            builder.Register<IGameShopService, GameShopService>(Lifetime.Singleton);
        }
    }
}