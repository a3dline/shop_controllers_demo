using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class BuildInSceneReference : ISceneReference
    {
        private readonly string _sceneName;
        private Scene _scene;

        public BuildInSceneReference(string sceneName)
        {
            _sceneName = sceneName;
        }

        public async UniTask<Scene> LoadAsync(LoadSceneMode mode, CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(_sceneName, mode).ToUniTask(cancellationToken: token);
            _scene = SceneManager.GetSceneByName(_sceneName);
            return _scene;
        }

        public UniTask DisposeAsync()
        {
            // Skip unloading a scene in EditMode because the scene is already unloaded when stopping PlayMode
            if (!Application.isPlaying)
            {
                return UniTask.CompletedTask;
            }
            
            return SceneManager.UnloadSceneAsync(_scene).ToUniTask();
        }
    }
}