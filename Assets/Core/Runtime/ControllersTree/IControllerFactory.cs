namespace Core.Controllers
{
    public interface IControllerFactory
    {
        T Create<T>();
    }
}