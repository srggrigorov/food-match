using Cysharp.Threading.Tasks;
using FoodMatch.Game;
using FoodMatch.Settings;
using FoodMatch.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace FoodMatch.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneSwitcher _sceneSwitcher;
        public override void InstallBindings()
        {
            Container.Bind<AssetsManager>().AsSingle().NonLazy();
            Container.Bind<SceneSwitcher>().FromInstance(_sceneSwitcher).AsSingle().NonLazy();
        }

        async private void Awake()
        {
            var assetsManager = Container.Resolve<AssetsManager>();
            await assetsManager.InitializeAsync();
            await assetsManager.LoadModulesSettings();

            _sceneSwitcher.SwitchSceneAsync(ScenesNames.Menu, LoadSceneMode.Single, container =>
                {
                    container.Bind<AssetsManager>().FromInstance(assetsManager).AsSingle().NonLazy();
                }).Forget();
        }
    }
}
