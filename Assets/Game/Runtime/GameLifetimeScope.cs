using Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ScriptableGameContext _gameContext;

        protected override void Configure(IContainerBuilder builder)
        {
            CoreInstaller.Install(builder);
            builder.RegisterEntryPoint<GameEntryPoint>()
                   .WithParameter(_gameContext.Context);
            builder.Register<GameRootController>(Lifetime.Transient);
        }
    }
}