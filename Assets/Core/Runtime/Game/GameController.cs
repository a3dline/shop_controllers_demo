using System.Threading;
using Core.Controllers;
using Core.ControllersTree;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.Game
{
    public class GameScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<GameController>(Lifetime.Transient);
        }
    }
    
    public class GameController : Controller
    {
        public GameController(IControllerFactory controllerFactory) : base(controllerFactory) { }
        protected override UniTask<ControllerEmptyResult> AsyncFlow(CancellationToken flowToken)
        {
            throw new System.NotImplementedException();
        }
    }
}