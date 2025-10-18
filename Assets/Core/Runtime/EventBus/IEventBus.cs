using Cysharp.Threading.Tasks;

namespace Core.EventsBus
{
    public interface IEventBus
    {
        void RaiseEvent(IBusEvent e);
        IUniTaskAsyncEnumerable<TEvent> Subscribe<TEvent>() where TEvent : IBusEvent;
    }
}