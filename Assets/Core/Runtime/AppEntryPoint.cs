using System.Threading;
using Core.Disposables;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class AppEntryPoint : DisposableBase, IStartable
    {
        private readonly IObjectResolver _resolver;
        private CancellationTokenSource _appTokenSource;


        public AppEntryPoint(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void Start()
        {
            _appTokenSource = new CancellationTokenSource();
            var rootController = _resolver.Resolve<AppRootController>();
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