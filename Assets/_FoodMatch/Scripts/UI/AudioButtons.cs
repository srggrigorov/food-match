using FoodMatch.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FoodMatch.UI
{
    public class AudioButtons : MonoBehaviour
    {

        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Image[] _musicButtonImages;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Image[] _soundButtonImages;

        private SoundService _soundService;

        [Inject]
        public void Construct(SoundService soundService)
        {
            _soundService = soundService;
            _musicToggle.isOn = _soundService.IsMusicEnabled;
            _soundToggle.isOn = _soundService.IsSoundEnabled;
            ChangeImagesAlpha(_musicButtonImages, _musicToggle.isOn);
            ChangeImagesAlpha(_soundButtonImages, _soundToggle.isOn);
        }

        protected void OnEnable()
        {
            _musicToggle.onValueChanged.AddListener(EnableMusic);
            _soundToggle.onValueChanged.AddListener(EnableSound);
        }

        private void EnableMusic(bool value)
        {
            _soundService.EnableMusic(value);
            ChangeImagesAlpha(_musicButtonImages, value);
        }

        private void EnableSound(bool value)
        {
            _soundService.EnableSound(value);
            ChangeImagesAlpha(_soundButtonImages, value);
        }

        private void ChangeImagesAlpha(Image[] images, bool isEnabled)
        {
            foreach (var image in images)
            {
                Color newColor = image.color;
                newColor.a = isEnabled ? 1 : 0.3f;
                image.color = newColor;
            }
        }

        protected void OnDisable()
        {
            _musicToggle.onValueChanged.RemoveListener(EnableMusic);
            _soundToggle.onValueChanged.RemoveListener(EnableSound);
        }
    }
}
