using System.Threading;
using Core;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameEntryPoint : DisposableBase, IStartable
    {
        private readonly IObjectResolver _resolver;
        private CancellationTokenSource _appTokenSource;


        public GameEntryPoint(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void Start()
        {
            _appTokenSource = new CancellationTokenSource();
            var rootController = _resolver.Resolve<GameRootController>();
            rootController.LaunchTree(_appTokenSource.Token);
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