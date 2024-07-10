using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FoodMatch.Settings;
using UnityEngine;
namespace FoodMatch.Game
{
    public class SlotsViewService : IDisposable
    {
        public event Action<ItemType> OnTripletCleared;

        private readonly ItemCollectorService _collectorService;
        private readonly Slot[] _slots;
        private readonly ItemsTweensSettings _itemsTweensSettings;
        private readonly SoundService _soundService;
        private readonly Transform _itemsSpawnPoint;
        private readonly Dictionary<Item, Sequence> _itemsSequences = new Dictionary<Item, Sequence>();

        public SlotsViewService(ItemCollectorService collectorService, AssetsManager assetsManager,
            SoundService soundService, Transform itemsSpawnPoint, Slot[] slots)
        {
            (_collectorService, _slots, _soundService, _itemsSpawnPoint) = (collectorService, slots, soundService, itemsSpawnPoint);
            _itemsTweensSettings = assetsManager.GetModuleSettings<ItemsTweensSettings>();

            _collectorService.OnItemPicked += MoveItemsToSlots;
            _collectorService.OnTripletCollected += HandleTripleCollected;
            _collectorService.OnLastItemReturned += HandleLastItemReturned;
        }
        
        private void HandleLastItemReturned(Item lastItem)
        {
            if (_itemsSequences.ContainsKey(lastItem))
            {
                _itemsSequences.Remove(lastItem);
            }
            Sequence itemSequence = DOTween.Sequence();
            itemSequence.Append(lastItem.Transform.DOJump(
                _itemsSpawnPoint.position, _itemsTweensSettings.ItemJumpForce,
                1, _itemsTweensSettings.MoveToSlotDuration));

            itemSequence.Join(lastItem.Transform.DOScale(_itemsSpawnPoint.localScale, _itemsTweensSettings.MoveToSlotDuration));
            itemSequence.Play().OnComplete(() => lastItem.SetInteractable(true));
            MoveItemsToSlots();
        }
        private void HandleTripleCollected(Item collectedItem, int slotIndex)
        {
            _itemsSequences.TryGetValue(collectedItem, out var itemSequence);
            RemoveTripleAfterSequence(collectedItem, slotIndex, itemSequence).Forget();
        }

        async private UniTask RemoveTripleAfterSequence(Item lastItem, int slotIndex, Sequence collectedItemSequence)
        {
            if (collectedItemSequence != null)
            {
                await collectedItemSequence.AsyncWaitForCompletion();
            }

            Vector3 targetItemsPosition = _slots[slotIndex - 1].ItemContainer.position + Vector3.up * _itemsTweensSettings.ClearTripleHeight;
            Sequence tripleSequence = DOTween.Sequence();
            for (int i = slotIndex - 2; i <= slotIndex; i++)
            {
                Item item = _slots[i].Item;
                tripleSequence.Join(item.Transform.DOMove(targetItemsPosition, _itemsTweensSettings.MoveToSlotDuration)
                    .OnComplete(() =>
                        {
                            _soundService.PlaySoundOnce(_soundService.SoundClips.TripletCollected);
                            item.DestroyWithParticles();
                        }));

                _slots[i].Item = null;
            }
            tripleSequence.Play().OnComplete(() => OnTripletCleared?.Invoke(lastItem.ItemType));

            MoveItemsToSlots();
        }

        private void MoveItemsToSlots(Item _ = null)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                Slot targetSlot = _slots[i];
                Item targetItem = _collectorService.ItemsInSlots[i];
                if (targetSlot.Item == targetItem)
                {
                    continue;
                }
                targetSlot.Item = targetItem;
                if (targetItem == null)
                {
                    continue;
                }

                if (targetItem.IsInteractable)
                {
                    targetItem.ToggleSelected(false);
                    targetItem.SetInteractable(false);
                }

                Sequence itemSequence = DOTween.Sequence();
                itemSequence.Append(targetItem.Transform.DOJump(
                    targetSlot.ItemContainer.position, _itemsTweensSettings.ItemJumpForce,
                    1, _itemsTweensSettings.MoveToSlotDuration));

                itemSequence.Join(targetItem.Transform.DOScale(targetSlot.ItemContainer.localScale, _itemsTweensSettings.MoveToSlotDuration));
                itemSequence.Join(targetItem.Transform.DORotateQuaternion(targetSlot.ItemContainer.rotation,
                    _itemsTweensSettings.MoveToSlotDuration));

                _itemsSequences.TryAdd(targetItem, itemSequence);
                itemSequence.Play().OnComplete(() =>
                    {
                        if (_itemsSequences.ContainsKey(targetItem))
                        {
                            _itemsSequences.Remove(targetItem);
                        }
                    });
            }
        }

        public void Dispose()
        {
            _collectorService.OnItemPicked -= MoveItemsToSlots;
            _collectorService.OnTripletCollected -= HandleTripleCollected;
            _collectorService.OnLastItemReturned -= HandleLastItemReturned;
        }
    }
}
