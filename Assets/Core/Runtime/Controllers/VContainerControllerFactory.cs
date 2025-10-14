using VContainer;

namespace Core.Controllers
{
    internal class VContainerControllerFactory : IControllerFactory
    {
        private readonly IObjectResolver _resolver;

        public VContainerControllerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Create<T>()
        {
            return _resolver.Resolve<T>();
        }
    }
}