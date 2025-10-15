using System;
using System.Threading;
using Core.ControllersTree;
using Cysharp.Threading.Tasks;

namespace Core.Controllers
{
    public abstract class RootController : Controller
    {
        internal RootController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        public void LaunchTree(CancellationToken token, Action<Exception> exceptionHandler = null)
        {
            AsyncFlow(token).Forget(exceptionHandler);
        }
    }
}