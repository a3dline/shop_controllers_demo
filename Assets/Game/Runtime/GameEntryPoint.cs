using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Features.BalanceBar;
using Features.GameShop;
using VContainer.Unity;

namespace Game
{
    public class GameEntryPoint : RootControllerBase, IAsyncStartable
    {
        private readonly GameContext _gameContext;

        public GameEntryPoint(IControllerFactory controllerFactory,
                              GameContext gameContext)
            : base(controllerFactory)
        {
            _gameContext = gameContext;
        }


        public UniTask StartAsync(CancellationToken cancellation = default)
        {
            LaunchTree(cancellation);
            return UniTask.WaitUntilCanceled(cancellation);
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            this.StartAndWaitInScope<BalanceBarController, BalanceBarScope>(_gameContext.BalanceBarContext, flowToken).Forget();
            await this.StartAndWaitInScope<GameShopController, GameShopScope>(_gameContext.GameShopContext, flowToken);
        }
    }
}