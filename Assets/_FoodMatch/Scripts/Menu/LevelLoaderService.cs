using System;
using Cysharp.Threading.Tasks;
using FoodMatch.Game;
using FoodMatch.Settings;
using UnityEngine.SceneManagement;
using Zenject;
namespace FoodMatch.Menu
{
    public class LevelLoaderService
    {
        private readonly SceneSwitcher _sceneSwitcher;
        private readonly AssetsManager _assetsManager;
        private readonly LevelsSettings _levelsSettings;
        private readonly SoundService _soundService;

        public LevelLoaderService(SceneSwitcher sceneSwitcher, AssetsManager assetsManager, SoundService soundService)
        {
            (_sceneSwitcher, _assetsManager, _soundService) = (sceneSwitcher, assetsManager, soundService);
            _levelsSettings = assetsManager.GetModuleSettings<LevelsSettings>();
        }

        public async UniTask LoadLevelAsync(string levelId)
        {
            var levelSettings = _levelsSettings.SettingsList.Find(x => x.name == levelId);
            if (levelSettings == null)
            {
                throw new NullReferenceException($"There is no level settings with ID {levelId}!");
            }

            await _sceneSwitcher.SwitchSceneAsync(ScenesNames.Game, LoadSceneMode.Single, container =>
                {
                    container.Bind<SoundService>().FromInstance(_soundService).AsSingle().NonLazy();
                    container.Bind<AssetsManager>().FromInstance(_assetsManager).AsSingle().NonLazy();
                    container.Bind<LevelLoaderService>().FromInstance(this).AsSingle().NonLazy();
                    container.Bind<LevelSettings>().FromInstance(levelSettings).AsSingle().NonLazy();
                    container.BindFactory<ItemSpawnGroup, ItemSpawnGroup.Factory>().FromComponentInNewPrefab(levelSettings.SpawnGroup).NonLazy();
                });
        }
    }
}
