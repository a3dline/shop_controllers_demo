using Core;
using Features.GoldSku;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ScriptableGameContext _gameContext;

        private readonly IInstaller[] _installers =
        {
            new CoreInstaller(),
            
            // I can use reflection here to automatically register all sku installers (regarding the TD). However, it is a kill for performance.
            // I cannot use the [InitializeOnLoad] attribute because this demo-project setup to be run without a domain reload.
            // The workaround is to create a custom build step that will generate all installer types into a resource file.
            new GoldSkuInstaller()
        };

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameEntryPoint>()
                   .WithParameter(_gameContext.Context);
            
            foreach (var installer in _installers)
            {
                installer.Install(builder);
            }
        }
    }
}