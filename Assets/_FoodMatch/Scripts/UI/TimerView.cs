using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace FoodMatch.Game
{
    public class TimerView : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private TMP_Text _timeText;

        private TimerService _timerService;

        [Inject]
        private void Construct(TimerService timerService)
        {
            _timerService = timerService;
            _timerService.OnTimeChanged += HandleTimeChanged;
        }
        private void HandleTimeChanged(int newTimeSec)
        {
            int minutes = newTimeSec / 60;
            int seconds = newTimeSec % 60;
            _timeText.text = $"{minutes:00}:{seconds:00}";

            if (newTimeSec < 10)
            {
                _timeText.transform.DOPunchScale(Vector3.one * 0.25f, 0.9f, 0, 0);
            }
            _timeText.ForceMeshUpdate();
        }

        public void Dispose()
        {
            _timerService.OnTimeChanged -= HandleTimeChanged;
        }
    }
}
