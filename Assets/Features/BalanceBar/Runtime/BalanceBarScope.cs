using VContainer;
using VContainer.Unity;

namespace Features.BalanceBar
{
    public class BalanceBarScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<BalanceBarController>(Lifetime.Transient);
            builder.Register<BalanceBarItemController>(Lifetime.Transient);
            builder.Register<BalanceBarViewController>(Lifetime.Transient);
        }
    }
}