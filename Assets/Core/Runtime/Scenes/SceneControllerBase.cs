using System;
using System.Threading;
using AControllersTree;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core
{
    public abstract class SceneControllerBase : ControllerBase

    {
        private readonly ISceneProvider _sceneProvider;

        public SceneControllerBase(IControllerFactory controllerFactory,
                                   ISceneProvider sceneProvider) : base(controllerFactory)
        {
            _sceneProvider = sceneProvider;
        }

        protected sealed override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var (sceneName, loadSceneMode) = GetSceneNameAndMode(context);
            
            await using var reference = _sceneProvider.GetSceneReference(sceneName);
            var scene = await reference.LoadAsync(loadSceneMode, flowToken);
            if (!scene.IsValid())
            {
                throw new Exception($"Scene '{sceneName}' is not valid");
            }

            var sceneContext = scene.GetRootComponent<SceneContextBase>();
            await AsyncFlow(sceneContext, context, flowToken);
        }

        protected abstract UniTask AsyncFlow(SceneContextBase sceneContext,
                                             object context,
                                             CancellationToken flowToken);
        
        protected abstract (string, LoadSceneMode) GetSceneNameAndMode(object context);
    }
}