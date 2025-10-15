using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using VContainer;

namespace Core.Tests
{
    public class ControllerTests
    {
        private CancellationTokenSource _cts;
        private IObjectResolver _testScope;


        [SetUp]
        public void SetUp()
        {
            _cts = new CancellationTokenSource();
            var builder = new ContainerBuilder();
            builder.Register<IControllerFactory, VContainerControllerFactory>(Lifetime.Scoped);
            _testScope = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _testScope.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }

        // Single child controller

        [Test]
        public void StartController_ChildWasStarted()
        {
            var testController = new TestController();
            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestController>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var rootController = scope.Resolve<TestRootController<TestController>>();
            rootController.LaunchTree(_cts.Token);

            Assert.That(testController.WasStarted, Is.True);
        }

        [Test]
        public void ThrowExceptionInChildController_ExceptionHandled()
        {
            Exception exceptionThrown = null;
            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestControllerWithException>>(Lifetime.Transient);
                builder.Register<TestControllerWithException>(Lifetime.Transient);
            });

            var rootController = scope.Resolve<TestRootController<TestControllerWithException>>();
            rootController.LaunchTree(_cts.Token, ex => { exceptionThrown = ex; });

            Assert.That(exceptionThrown.Message, Is.EqualTo("Test exception"));
        }

        [Test]
        public void ThrowExceptionInChildController_ChildWasStopped()
        {
            var testController = new TestControllerWithException();
            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestControllerWithException>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var rootController = scope.Resolve<TestRootController<TestControllerWithException>>();
            rootController.LaunchTree(_cts.Token, _ => { });

            Assert.That(testController.WasStopped, Is.True);
        }

        // Nested child controller

        [Test]
        public void StartControllerWithNested_ChildAndNestedWereStarted()
        {
            var testController = new TestController();

            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestControllerWithNested<TestController>>>(Lifetime.Transient);
                builder.Register<TestControllerWithNested<TestController>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var root = scope.Resolve<TestRootController<TestControllerWithNested<TestController>>>();
            root.LaunchTree(_cts.Token);

            Assert.That(testController.WasStarted, Is.True);
        }

        [Test]
        public void NestedController_Throws_ExceptionHandledAndAllStopped()
        {
            Exception exceptionThrown = null;
            var testController = new TestControllerWithException();

            using var scope = _testScope.CreateScope(builder =>
            {
                builder
                    .Register<
                        TestRootController<TestControllerWithNested<TestControllerWithException>>>(Lifetime.Transient);
                builder.Register<TestControllerWithNested<TestControllerWithException>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var root = scope.Resolve<TestRootController<TestControllerWithNested<TestControllerWithException>>>();
            root.LaunchTree(_cts.Token, ex => { exceptionThrown = ex; });

            Assert.That(exceptionThrown, Is.Not.Null);
            Assert.That(exceptionThrown.Message, Is.EqualTo("Test exception"));
            Assert.That(testController.WasStopped, Is.True);
        }

        // Cancellation flow

        [Test]
        public void CancelRootController_ChildWasStopped()
        {
            var testController = new TestController();
            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestController>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var rootController = scope.Resolve<TestRootController<TestController>>();
            _cts.Cancel();
            rootController.LaunchTree(_cts.Token);

            Assert.That(testController.WasStopped, Is.True);
        }

        [Test]
        public void CancelRootController_NestedChildWasStopped()
        {
            var testController = new TestController();
            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootController<TestControllerWithNested<TestController>>>(Lifetime.Transient);
                builder.Register<TestControllerWithNested<TestController>>(Lifetime.Transient);
                builder.RegisterInstance(testController);
            });

            var root = scope.Resolve<TestRootController<TestControllerWithNested<TestController>>>();
            _cts.Cancel();
            root.LaunchTree(_cts.Token);

            Assert.That(testController.WasStopped, Is.True);
        }

        // Result tests
        [Test]
        public void StartControllerAndWaitResult_ChildReturnsResult()
        {
            var expectedResult = 42;

            using var scope = _testScope.CreateScope(builder =>
            {
                builder.Register<TestRootControllerWithResult<TestControllerWithResult>>(Lifetime.Transient);
                builder.Register<TestControllerWithResult>(Lifetime.Transient)
                       .WithParameter(expectedResult);
            });

            var root = scope.Resolve<TestRootControllerWithResult<TestControllerWithResult>>();
            root.LaunchTree(_cts.Token);

            Assert.That(root.Result, Is.EqualTo(expectedResult));
        }
    }

    public class TestRootController<TControllerForStart> : RootControllerBase
        where TControllerForStart : IController<ControllerEmptyResult>
    {
        internal TestRootController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask AsyncFlow(CancellationToken flowToken)
        {
            await StartAndWait<TControllerForStart>(flowToken);
        }
    }

    public class TestRootControllerWithResult<TControllerForStart> : RootControllerBase
        where TControllerForStart : IController<int>
    {
        internal TestRootControllerWithResult(IControllerFactory controllerFactory) : base(controllerFactory) { }
        public int Result { get; private set; }

        protected override async UniTask AsyncFlow(CancellationToken flowToken)
        {
            Result = await StartAndWaitResult<TControllerForStart, int>(flowToken);
        }
    }

    public class TestControllerWithNested<TNestedController> : ControllerBase
        where TNestedController : IController<ControllerEmptyResult>
    {
        public TestControllerWithNested(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override async UniTask AsyncFlow(CancellationToken flowToken)
        {
            await StartAndWait<TNestedController>(flowToken);
        }
    }

    public class TestController : IController<ControllerEmptyResult>
    {
        public bool WasStarted { get; private set; }
        public bool WasStopped { get; private set; }

        public UniTask<ControllerEmptyResult> RunAsyncFlow(CancellationToken flowToken)
        {
            WasStarted = true;
            return UniTask.FromResult(new ControllerEmptyResult());
        }

        void IController.StopSelf()
        {
            WasStopped = true;
        }
    }

    public class TestControllerWithResult : IController<int>
    {
        private readonly int _result;

        public TestControllerWithResult(int result)
        {
            _result = result;
        }

        public UniTask<int> RunAsyncFlow(CancellationToken flowToken)
        {
            return UniTask.FromResult(_result);
        }

        void IController.StopSelf() { }
    }

    public class TestControllerWithException : IController<ControllerEmptyResult>
    {
        public bool WasStopped { get; private set; }

        public UniTask<ControllerEmptyResult> RunAsyncFlow(CancellationToken flowToken)
        {
            throw new Exception("Test exception");
        }

        void IController.StopSelf()
        {
            WasStopped = true;
        }
    }
}