using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Pool;
using VContainer;

namespace Core
{
    public abstract class ControllerBase : ControllerBase<ControllerEmptyResult>
    {
        public ControllerBase(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected sealed override async UniTask<ControllerEmptyResult> AsyncFlowWithResult(
            object context,
            CancellationToken flowToken)
        {
            await AsyncFlow(context, flowToken);
            return default;
        }

        protected abstract UniTask AsyncFlow(object context, CancellationToken flowToken);
    }

    public abstract partial class ControllerBase<TResult> : IController<TResult>
    {
        private readonly List<IController> _children;
        private readonly IControllerFactory _controllerFactory;

        private readonly CompositeDisposable _disposables = new();

        // Inject it as internal to prevent any unpredictable direct access from user code
        // Tip: Use VContainer roslin source generator for production build to avoid reflection 
        [Inject]
        internal readonly IObjectResolver Resolver;

        private object _context;

        [Inject]
        public ControllerBase(IControllerFactory controllerFactory)
        {
            _controllerFactory = controllerFactory;
            _disposables.Add(ListPool<IController>.Get(out _children));
        }

        // Only for testing
        internal ControllerBase(IControllerFactory controllerFactory, IObjectResolver resolver)
            : this(controllerFactory)
        {
            Resolver = resolver;
        }

        UniTask<TResult> IController<TResult>.RunAsyncFlow(object context, CancellationToken flowToken)
        {
            _context = context;
            return AsyncFlowWithResult(context, flowToken);
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

        internal async UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
            object context,
            IControllerFactory factory,
            CancellationToken token)
            where TController : IController<TControllerResult>
        {
            var child = factory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationRegistry = cts.Token.Register(() => { child.StopSelf(); });
            TControllerResult result;

            try
            {
                result = await child.RunAsyncFlow(context ?? _context, cts.Token);
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

            return result;
        }

        internal async UniTask StartAndWait<TController>(
            object context,
            IControllerFactory factory,
            CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            var child = factory.Create<TController>();
            _children.Add(child);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var cancellationRegistry = cts.Token.Register(() => { child.StopSelf(); });

            try
            {
                await child.RunAsyncFlow(context ?? _context, cts.Token);
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
        
        protected void RegisterDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        protected abstract UniTask<TResult> AsyncFlowWithResult(object context, CancellationToken flowToken);
        protected virtual void OnStop() { }
    }
}