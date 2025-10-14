using System.Collections.Generic;
using System.Threading;
using Core.Disposables;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;

namespace Core.Controllers
{
    public abstract class Controller : Controller<ControllerEmptyResult>
    {
        internal Controller(IControllerFactory controllerFactory) : base(controllerFactory) { }
    }

    public abstract class Controller<TResult> : IController<TResult>
    {
        private readonly List<IController> _children;
        private readonly IControllerFactory _controllerFactory;
        private readonly CompositeDisposable _disposables = new();

        internal Controller(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
            _disposables.Add(ListPool<IController>.Get(out _children));
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

        protected async UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
            CancellationToken token)
            where TController : IController<TControllerResult>
        {
            var child = _controllerFactory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            TControllerResult result;

            try
            {
                var t1 = UniTask.WaitUntilCanceled(cts.Token).ContinueWith(() => (UniTask<TControllerResult>)default);
                var t2 = child.RunAsyncFlow(cts.Token);
                result = await UniTask.WhenAny(t1, t2)
                                      .ContinueWith(x => x.winArgumentIndex == 0 ? x.result1 : x.result2);
            }
            finally
            {
                cts.Cancel();
                child.StopSelf();
            }

            return result;
        }

        protected async UniTask StartAndWait<TController>(CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            var child = _controllerFactory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);

            try
            {
                var t1 = UniTask.WaitUntilCanceled(cts.Token);
                var t2 = child.RunAsyncFlow(cts.Token);
                await UniTask.WhenAny(t1, t2);
            }
            finally
            {
                cts.Cancel();
                child.StopSelf();
            }
        }

        protected abstract UniTask<TResult> AsyncFlow(CancellationToken flowToken);
        protected virtual void OnStop() { }
    }
}