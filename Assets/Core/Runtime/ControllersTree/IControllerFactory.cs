namespace Core
{
    public interface IControllerFactory
    {
        T Create<T>();
    }
}