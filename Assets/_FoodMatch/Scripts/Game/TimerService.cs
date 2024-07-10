using System;
using Cysharp.Threading.Tasks;
using FoodMatch.Settings;
using Zenject;

namespace FoodMatch.Game
{
    //System.Threading.Timers and System.Timers.Timer aren't working with TMP for some reason
    public class TimerService : IInitializable
    {
        public event Action OnTimeRanOut;
        public event Action<int> OnTimeChanged;

        private int _totalSeconds;
        private bool _isRunning;

        public TimerService(LevelSettings levelSettings)
        {
            _totalSeconds = levelSettings.TimeSeconds;
        }

        public void Initialize()
        {
            OnTimeChanged?.Invoke(_totalSeconds);
            StartTimerAsync().Forget();
        }

        public void StopTimer() => _isRunning = false;

        async private UniTask StartTimerAsync()
        {
            _isRunning = true;
            while (_isRunning)
            {
                await UniTask.Delay(1000);
                _totalSeconds -= 1;
                OnTimeChanged?.Invoke(_totalSeconds);

                if (_totalSeconds <= 0)
                {
                    OnTimeRanOut?.Invoke();
                    StopTimer();
                }
            }
        }

    }
}
