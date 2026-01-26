using System.Threading;
using AControllersTree;
using Core;
using Core.EventsBus;
using Cysharp.Threading.Tasks;
using GameEvents.Runtime;

namespace Features.BalanceBar
{
    public class BalanceBarController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public BalanceBarController(IControllerFactory controllerFactory,
                                    IEventBus eventBus)
            : base(controllerFactory)
        {
            _eventBus = eventBus;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var balanceBarContext = (BalanceBarContext)context;

            await foreach (var evn in _eventBus.Subscribe<DisplayBalanceBarEvent>().WithCancellation(flowToken))
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(evn.DisplayToken, flowToken);
                var viewContext = new BalanceBarViewContext
                                  {
                                      Parent = evn.Parent,
                                      BalanceBarPrefabAddress = balanceBarContext.BalanceBarPrefabAddress
                                  };
                await StartAndWait<BalanceBarViewController>(viewContext, cts.Token);
            }
        }
    }
}