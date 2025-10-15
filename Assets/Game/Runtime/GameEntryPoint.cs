using System.Threading;
using Core;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameEntryPoint : DisposableBase, IStartable
    {
        private readonly IObjectResolver _resolver;
        private readonly IControllerFactory _controllerFactory;
        private readonly GameContext _gameContext;
        private CancellationTokenSource _appTokenSource;


        public GameEntryPoint(IObjectResolver resolver,
                              GameContext gameContext)
        {
            _resolver = resolver;
            _gameContext = gameContext;
        }

        public void Start()
        {
            _appTokenSource = new CancellationTokenSource();
            
            var rootController = _resolver.Resolve<GameRootController>();
            rootController.LaunchTree(_gameContext, _appTokenSource.Token);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appTokenSource.Cancel();
                _appTokenSource.Dispose();
            }
        }
    }
}