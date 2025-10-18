using VContainer;
using VContainer.Unity;

namespace Features.GoldSku
{
    public abstract class StaticScopeBase : IInstaller
    {
        private IScopedObjectResolver _scope;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(resolver => { _scope = resolver.CreateScope(Configure); });

            builder.RegisterDisposeCallback(_ =>
            {
                _scope?.Dispose();
                _scope = null;
            });
        }

        protected abstract void Configure(IContainerBuilder builder);
    }
}