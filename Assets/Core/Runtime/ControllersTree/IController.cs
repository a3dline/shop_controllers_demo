using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IController<TResult> : IController
    {
        UniTask<TResult> RunAsyncFlow(CancellationToken flowToken);
    }

    public interface IController
    {
        internal void StopSelf();
    }
}