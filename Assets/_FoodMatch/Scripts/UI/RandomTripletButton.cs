using System;
using FoodMatch.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FoodMatch.UI
{
    public class RandomTripletButton : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Image _buttonIcon;

        private EndgameService _endgameService;
        private RandomTripletCollectorService _tripletCollectorService;

        [Inject]
        private void Construct(RandomTripletCollectorService tripletCollectorService, EndgameService endgameService)
        {
            (_tripletCollectorService, _endgameService) = (tripletCollectorService, endgameService);
        }

        private void Awake()
        {
            _button.onClick.AddListener(RemoveRandomTriplet);
            _tripletCollectorService.OnRandomTripletCollected += HandleTripletCollected;
            _endgameService.OnGameEnded += DisableButton;
        }
        private void HandleTripletCollected(ItemType _) => DisableButton();

        private void RemoveRandomTriplet() => _tripletCollectorService.CollectRandomTriplet();

        private void DisableButton()
        {
            Unsubscribe();
            _button.interactable = false;
            _buttonIcon.color = _button.colors.disabledColor;
        }

        public void Dispose() => Unsubscribe();

        private void Unsubscribe()
        {
            _button.onClick.RemoveListener(RemoveRandomTriplet);
            _endgameService.OnGameEnded -= DisableButton;
            _tripletCollectorService.OnRandomTripletCollected -= HandleTripletCollected;
        }
    }
}
