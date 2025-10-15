using System.Collections.Generic;
using System.Threading;
using Core.Controllers;
using Core.Disposables;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using VContainer;

namespace Core.ControllersTree
{
    public abstract class Controller : Controller<ControllerEmptyResult>
    {
        internal Controller(IControllerFactory controllerFactory) : base(controllerFactory) { }
    }

    public abstract partial class Controller<TResult> : IController<TResult>
    {
        private readonly List<IController> _children;
        private readonly IControllerFactory _controllerFactory;

        private readonly CompositeDisposable _disposables = new();

        // Inject it we'd like to prevent any unpredictable direct access from user code
        // Tip: Use VContainer roslin source generator for production build to avoid reflection 
        [Inject]
        private readonly IObjectResolver _resolver;

        [Inject]
        internal Controller(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
            _disposables.Add(ListPool<IController>.Get(out _children));
        }

        // Only for testing
        internal Controller(IControllerFactory controllerFactory, IObjectResolver resolver)
            : this(controllerFactory)
        {
            _resolver = resolver;
        }

        UniTask<TResult> IController<TResult>.RunAsyncFlow(CancellationToken flowToken)
        {
            return AsyncFlow(flowToken);
        }


        void IController.StopSelf()
        {
            try
            {
                OnStop();
            }
            finally
            {
                _disposables.Dispose();
            }
        }

        protected abstract UniTask<TResult> AsyncFlow(CancellationToken flowToken);
        protected virtual void OnStop() { }

        private async UniTask StartAndWait<TController>(IControllerFactory factory, CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            var child = factory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationRegistry = cts.Token.Register(() => { child.StopSelf(); });

            try
            {
                await child.RunAsyncFlow(cts.Token);
            }
            finally
            {
                cancellationRegistry.Dispose();

                if (!cts.IsCancellationRequested)
                {
                    cts.Cancel();
                    child.StopSelf();
                }
            }
        }
    }
}