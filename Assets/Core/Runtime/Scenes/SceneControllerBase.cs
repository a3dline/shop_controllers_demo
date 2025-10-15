using System;
using System.Threading;
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

        protected abstract string SceneName { get; }
        protected abstract LoadSceneMode LoadSceneMode { get; }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            await using var reference = _sceneProvider.GetSceneReference(SceneName);
            var scene = await reference.LoadAsync(LoadSceneMode, flowToken);
            if (!scene.IsValid())
            {
                throw new Exception($"Scene '{SceneName}' is not valid");
            }

            var sceneContext = scene.GetRootComponent<SceneContextBase>();
            await AsyncFlow(sceneContext, context, flowToken);
        }

        protected abstract UniTask AsyncFlow(SceneContextBase sceneContext, object context, CancellationToken flowToken);
    }
}