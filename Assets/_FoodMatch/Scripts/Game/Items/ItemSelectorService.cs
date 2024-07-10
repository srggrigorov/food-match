using System;
using FoodMatch.Input;
using UnityEngine;

namespace FoodMatch.Game
{
    public class ItemSelectorService : IDisposable
    {
        public event Action<Item> OnItemPicked;

        private readonly Camera _levelCamera;
        private readonly InputService _inputService;
        private Item _lastSelectedItem;

        public ItemSelectorService(InputService inputService, Camera levelCamera)
        {
            (_inputService, _levelCamera) = (inputService, levelCamera);

            _inputService.OnTouchMoved += CheckForItemSelection;
            _inputService.OnTouchReleased += PickSelectedItem;
        }
        private void CheckForItemSelection(Vector2 touchPosition)
        {
            Ray selectRay = _levelCamera.ScreenPointToRay(touchPosition);
            
            if (!Physics.Raycast(selectRay, out RaycastHit hitInfo, 8) ||
                !hitInfo.transform.TryGetComponent(out Item item) ||
                !item.IsInteractable)
            {
                DisableLastSelection();
                _lastSelectedItem = null;
                return;
            }
            
            if (item == _lastSelectedItem)
            {
                return;
            }

            DisableLastSelection();
            item.ToggleSelected(true);
            _lastSelectedItem = item;
        }

        private void PickSelectedItem(Vector2 _)
        {
            if (_lastSelectedItem != null)
            {
                OnItemPicked?.Invoke(_lastSelectedItem);
            }
        }

        private void DisableLastSelection()
        {
            if (_lastSelectedItem != null && _lastSelectedItem.IsInteractable)
            {
                _lastSelectedItem.ToggleSelected(false);
            }
        }
        public void Dispose()
        {
            _inputService.OnTouchReleased -= PickSelectedItem;
            _inputService.OnTouchMoved -= CheckForItemSelection;
        }
    }
}
