using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core
{
    public interface ISceneReference : IUniTaskAsyncDisposable
    {
        UniTask<Scene> LoadAsync(LoadSceneMode mode, CancellationToken token);
    }
}