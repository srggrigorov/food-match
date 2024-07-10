using System;
using Cysharp.Threading.Tasks;
using FoodMatch.Input;
using FoodMatch.Settings;
using FoodMatch.UI;
using UnityEngine.SceneManagement;
using Zenject;

namespace FoodMatch.Game
{
    public class EndgameService : IDisposable
    {
        public event Action OnGameEnded;

        private readonly InputService _inputService;
        private readonly ItemCollectorService _collectorService;
        private readonly TimerService _timerService;
        private readonly LevelGoalService _goalService;
        private readonly SceneSwitcher _sceneSwitcher;
        private readonly AssetsManager _assetsManager;
        private readonly EndgameScreenView _endgameScreen;
        private readonly SoundService _soundService;

        public EndgameService(InputService inputService, ItemCollectorService collectorService,
            TimerService timerService, SoundService soundService,
            LevelGoalService goalService, SceneSwitcher sceneSwitcher,
            AssetsManager assetsManager, EndgameScreenView endgameScreen)
        {
            (_inputService, _collectorService, _timerService, _goalService, _sceneSwitcher, _assetsManager, _endgameScreen) =
                (inputService, collectorService, timerService, goalService, sceneSwitcher, assetsManager, endgameScreen);

            _soundService = soundService;

            _collectorService.OnDefeat += TriggerDefeat;
            _timerService.OnTimeRanOut += TriggerDefeat;
            _goalService.AllGoalsAchieved += TriggerVictory;
        }

        async private UniTask ShowEndgameScreen(bool isVictory)
        {
            _inputService.Disable();
            _timerService.StopTimer();
            OnGameEnded?.Invoke();
            await _endgameScreen.Open(isVictory);
        }

        private void TriggerVictory() => ShowEndgameScreen(true).Forget();

        private void TriggerDefeat() => ShowEndgameScreen(false).Forget();

        public void Dispose()
        {
            _collectorService.OnDefeat -= TriggerDefeat;
            _timerService.OnTimeRanOut -= TriggerDefeat;
            _goalService.AllGoalsAchieved -= TriggerVictory;
        }

        public async UniTask ReturnToMenu()
        {
            await _sceneSwitcher.SwitchSceneAsync(ScenesNames.Menu, LoadSceneMode.Single, container =>
                {
                    container.Bind<AssetsManager>().FromInstance(_assetsManager).AsSingle().NonLazy();
                    container.Bind<SoundService>().FromInstance(_soundService).AsSingle().NonLazy();
                });
        }
    }
}
