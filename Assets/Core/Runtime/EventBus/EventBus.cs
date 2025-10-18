using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace Core.EventsBus
{
    internal class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<ISubscription>> _subscriptions = new();
        private readonly List<ISubscription> _subscriptionsToRemove = new();

        public void RaiseEvent(IBusEvent e)
        {
            var type = e.GetType();
            foreach (var (sType, list) in _subscriptions)
            {
                if (!sType.IsAssignableFrom(type))
                {
                    continue;
                }

                foreach (var s in list)
                {
                    s.Raise(e);
                }
            }

            foreach (var s in _subscriptionsToRemove)
            {
                foreach (var list in _subscriptions.Values) 
                {
                    list.Remove(s);
                }
            }

            _subscriptionsToRemove.Clear();
        }

        public IUniTaskAsyncEnumerable<TEvent> Subscribe<TEvent>() where TEvent : IBusEvent
        {
            return UniTaskAsyncEnumerable.Create<TEvent>(async (writer, token) =>
            {
                var type = typeof(TEvent);

                if (!_subscriptions.TryGetValue(type, out var list))
                {
                    list = new List<ISubscription>();
                    _subscriptions[type] = list;
                }

                var subscription = new Subscription<TEvent>();
                list.Add(subscription);

                try
                {
                    await subscription.Run(writer, token);
                }
                finally
                {
                    subscription.Complete();
                    AddToRemove(subscription);
                }
            });
        }

        private void AddToRemove(ISubscription subscription)
        {
            _subscriptionsToRemove.Add(subscription);
        }

        private interface ISubscription
        {
            void Raise(IBusEvent busEvent);
        }

        private sealed class Subscription<TEvent> : ISubscription
            where TEvent : IBusEvent
        {
            private readonly Queue<TEvent> _queue = new();
            private bool _complete;
            private UniTaskCompletionSource _waiter;

            public void Raise(IBusEvent busEvent)
            {
                if (_complete)
                {
                    return;
                }

                _queue.Enqueue((TEvent)busEvent);
                _waiter?.TrySetResult();
            }

            public async UniTask Run(IAsyncWriter<TEvent> writer, CancellationToken token)
            {
                try
                {
                    while (!token.IsCancellationRequested && !_complete)
                    {
                        while (_queue.Count > 0)
                        {
                            await writer.YieldAsync(_queue.Dequeue());
                        }

                        _waiter = UniTaskSourceCached.Get();
                        await _waiter.Task.AttachExternalCancellation(token);
                        _waiter = null;
                    }
                }
                catch (OperationCanceledException)
                {
                    // ignore propagation
                }
            }

            public void Complete()
            {
                _complete = true;
                _waiter?.TrySetCanceled();
            }

            private static class UniTaskSourceCached
            {
                // ReSharper disable once StaticMemberInGenericType
                private static UniTaskCompletionSource _cached;

                public static UniTaskCompletionSource Get()
                {
                    var t = _cached ?? new UniTaskCompletionSource();
                    _cached = null;
                    return t;
                }
            }
        }
    }
}