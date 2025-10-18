using System;
using Features.BalanceBar;
using Features.GameShop;

namespace Game
{
    [Serializable]
    public class GameContext
    {
        public GameShopContext GameShopContext;
        public BalanceBarContext BalanceBarContext;
    }
}