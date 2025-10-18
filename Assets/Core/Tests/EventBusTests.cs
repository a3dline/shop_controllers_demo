using System.Threading;
using System.Threading.Tasks;
using Core.EventsBus;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace Core.Tests
{
    public class EventBusTests
    {
        private EventBus _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new EventBus();
        }

        [Test]
        public async Task Test_EventSubscription_ReceivesEvents()
        {
            var received = 0;
            var cts = new CancellationTokenSource();

            async UniTask _Listen()
            {
                await foreach (var e in _bus.Subscribe<TestEvent>().WithCancellation(cts.Token))
                {
                    received += e.Value;
                }
            }

            _Listen().Forget();

            _bus.RaiseEvent(new TestEvent { Value = 5 });
            _bus.RaiseEvent(new TestEvent { Value = 10 });

            await UniTask.Delay(1, cancellationToken: cts.Token);
            cts.Cancel();

            Assert.AreEqual(15, received);
        }


        [Test]
        public async Task Test_EventSubscription_BaseType_ReceivesDerivedEvents()
        {
            var received = 0;
            var cts = new CancellationTokenSource();

            async UniTask _Listen()
            {
                await foreach (var e in _bus.Subscribe<BaseEvent>().WithCancellation(cts.Token))
                {
                    received += e.Value;
                }
            }

            _Listen().Forget();

            _bus.RaiseEvent(new DerivedEvent { Value = 7 });

            await UniTask.Delay(1, cancellationToken: cts.Token);
            cts.Cancel();

            Assert.AreEqual(7, received);
        }

        [Test]
        public async Task Test_EventSubscription_CancelAfterFirst()
        {
            var received = 0;
            var cts = new CancellationTokenSource();

            async UniTask _Listen()
            {
                await foreach (var e in _bus.Subscribe<TestEvent>().WithCancellation(cts.Token))
                {
                    received += e.Value;
                    cts.Cancel();
                }
            }

            _Listen().Forget();

            _bus.RaiseEvent(new TestEvent { Value = 5 });
            _bus.RaiseEvent(new TestEvent { Value = 10 });

            await UniTask.Delay(1, cancellationToken: cts.Token);

            Assert.AreEqual(5, received);
        }

        [Test]
        public async Task Test_EventSubscription_MultipleSubscribers()
        {
            var a = 0;
            var b = 0;
            var cts = new CancellationTokenSource();

            async UniTask _ListenA()
            {
                await foreach (var e in _bus.Subscribe<TestEvent>().WithCancellation(cts.Token))
                {
                    a += e.Value;
                }
            }

            async UniTask _ListenB()
            {
                await foreach (var e in _bus.Subscribe<TestEvent>().WithCancellation(cts.Token))
                {
                    b += e.Value;
                }
            }

            _ListenA().Forget();
            _ListenB().Forget();

            _bus.RaiseEvent(new TestEvent { Value = 3 });

            await UniTask.Delay(1, cancellationToken: cts.Token);
            cts.Cancel();

            Assert.AreEqual(3, a);
            Assert.AreEqual(3, b);
        }

        private class TestEvent : IBusEvent
        {
            public int Value;
        }

        private class BaseEvent : IBusEvent
        {
            public int Value;
        }

        private class DerivedEvent : BaseEvent { }
    }
}