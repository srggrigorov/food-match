using System;
using FoodMatch.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FoodMatch.UI
{
    public class ReturnItemButton : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Image _buttonIcon;

        private ItemCollectorService _collectorService;
        private EndgameService _endgameService;

        [Inject]
        private void Construct(ItemCollectorService collectorService, EndgameService endgameService)
        {
            (_collectorService, _endgameService) = (collectorService, endgameService);
        }

        private void Awake()
        {
            _button.onClick.AddListener(ReturnLastItem);
            _collectorService.OnLastItemReturned += HandleItemReturned;
            _endgameService.OnGameEnded += DisableButton;
        }
        private void HandleItemReturned(Item _) => DisableButton();

        private void ReturnLastItem() => _collectorService.ReturnLastItem();

        private void DisableButton()
        {
            Unsubscribe();
            _button.interactable = false;
            _buttonIcon.color = _button.colors.disabledColor;
        }

        public void Dispose() => Unsubscribe();

        private void Unsubscribe()
        {
            _button.onClick.RemoveListener(ReturnLastItem);
            _endgameService.OnGameEnded -= DisableButton;
        }
    }
}
