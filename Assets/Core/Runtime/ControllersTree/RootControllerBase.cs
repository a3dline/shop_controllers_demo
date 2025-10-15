using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public abstract class RootControllerBase : ControllerBase
    {
        public RootControllerBase(IControllerFactory controllerFactory) : base(controllerFactory) { }

        public void LaunchTree(CancellationToken token, Action<Exception> exceptionHandler = null)
        {
            AsyncFlow(token).Forget(exceptionHandler);
        }
    }
}