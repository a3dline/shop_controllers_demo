using Game.Shop;
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
        }
    }
}