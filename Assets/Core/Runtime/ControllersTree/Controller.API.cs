using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract partial class ControllerBase<TResult>
    {
        public UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
            CancellationToken token)
            where TController : IController<TControllerResult>
        {
            return StartAndWaitResult<TController, TControllerResult>(null, _controllerFactory, token);
        }
        
        public UniTask<TControllerResult> StartAndWaitResult<TController, TControllerResult>(
            object context,
            CancellationToken token)
            where TController : IController<TControllerResult>
        {
            return StartAndWaitResult<TController, TControllerResult>(context, _controllerFactory, token);
        }

        public UniTask StartAndWait<TController>(CancellationToken token) 
            where TController : IController<ControllerEmptyResult>
        {
            return StartAndWait<TController>(null, token);
        }
        
        public UniTask StartAndWait<TController>(object context, CancellationToken token)
            where TController : IController<ControllerEmptyResult>
        {
            return StartAndWait<TController>(context, _controllerFactory, token);
        }
    }
}