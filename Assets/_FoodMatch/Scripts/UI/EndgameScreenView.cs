using Cysharp.Threading.Tasks;
using DG.Tweening;
using FoodMatch.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FoodMatch.UI
{
    public class EndgameScreenView : MonoBehaviour
    {
        [SerializeField]
        private Button _menuButton;
        [SerializeField]
        private Image _fadeInImage;
        [SerializeField]
        private Transform _buttonsContainer;

        [Space]
        [SerializeField]
        private float _fadeInTargetAlpha;

        [Header("Durations")]
        [SerializeField]
        private float _openingDelayDuration;
        [SerializeField]
        private float _fadeInDuration;
        [SerializeField]
        private float _buttonsMoveDuration;
        [SerializeField]
        private float _screenZoomDuration;
        [SerializeField]
        private Transform _defeatScreen;
        [SerializeField]
        private Transform _victoryScreen;

        private EndgameService _endgameService;

        [Inject]
        private void Construct(EndgameService endgameService)
        {
            _endgameService = endgameService;
        }

        public async UniTask Open(bool isVictory)
        {
            await UniTask.WaitForSeconds(_openingDelayDuration);
            gameObject.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_fadeInImage.DOFade(_fadeInTargetAlpha, _fadeInDuration));
            sequence.Join(_menuButton.transform.DOMove(_buttonsContainer.transform.position, _buttonsMoveDuration));
            Transform targetScreen = isVictory ? _victoryScreen : _defeatScreen;
            sequence.Join(targetScreen.DOScale(Vector3.one, _screenZoomDuration));
            sequence.Play().OnComplete(() =>
                {
                    _menuButton.onClick.AddListener(() => _endgameService.ReturnToMenu().Forget());
                });
        }
    }
}
