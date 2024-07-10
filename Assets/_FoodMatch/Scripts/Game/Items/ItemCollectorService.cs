using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodMatch.Game
{
    public class ItemCollectorService : IDisposable
    {
        public delegate void TripletCollectedDelegate(Item lastItem, int slotIndexOfLastItem);
        public event Action<Item> OnItemPicked;
        public event TripletCollectedDelegate OnTripletCollected;
        public event Action<Item> OnLastItemReturned;
        public event Action OnDefeat;

        public IReadOnlyList<Item> ItemsInSlots => _itemsInSlots;

        private const int ItemSlotsCount = 7;
        private readonly ItemSelectorService _selectorService;
        private readonly SoundService _soundService;
        private readonly Item[] _itemsInSlots = new Item[ItemSlotsCount];
        private Item _lastCollectedItem;

        public ItemCollectorService(ItemSelectorService selectorService, SoundService soundService)
        {
            (_selectorService, _soundService) = (selectorService, soundService);
            _selectorService.OnItemPicked += HandleItemPicked;
        }
        private void HandleItemPicked(Item item)
        {
            if (item is Dynamite bomb)
            {
                _soundService.PlaySoundOnce(_soundService.SoundClips.Explosion);
                bomb.Explode();
                OnDefeat?.Invoke();
                return;
            }

            Item lastSimilarItem = _itemsInSlots.LastOrDefault(x => x != null && x.ItemType == item.ItemType);
            int indexOfItem;

            if (lastSimilarItem == null)
            {
                //Placing an item in the first free space
                indexOfItem = Array.IndexOf(_itemsInSlots, null);
                _itemsInSlots[indexOfItem] = item;
            }
            else
            {
                //Moving other items forward and placing an item after a similar one
                int indexOfLast = Array.LastIndexOf(_itemsInSlots, lastSimilarItem);
                if (_itemsInSlots[indexOfLast + 1] != null)
                {
                    for (int i = _itemsInSlots.Length - 2; i >= indexOfLast; i--)
                    {
                        _itemsInSlots[i + 1] = _itemsInSlots[i];
                    }
                }
                indexOfItem = indexOfLast + 1;
                _itemsInSlots[indexOfItem] = item;
            }

            _lastCollectedItem = item;
            OnItemPicked?.Invoke(item);
            _soundService.PlaySoundOnce(_soundService.SoundClips.PickedItem);

            CheckForTripleItems(item, indexOfItem);
        }

        public void ReturnLastItem()
        {
            if (_lastCollectedItem == null)
            {
                return;
            }
            int indexOfItem = Array.IndexOf(_itemsInSlots, _lastCollectedItem);
            _itemsInSlots[indexOfItem] = null;
            for (int i = indexOfItem + 1; i < _itemsInSlots.Length; i++)
            {
                if (_itemsInSlots[i] == null && _itemsInSlots[i - 1] == null)
                {
                    break;
                }
                _itemsInSlots[i - 1] = _itemsInSlots[i];
            }
            OnLastItemReturned?.Invoke(_lastCollectedItem);
            _lastCollectedItem = null;
        }

        private void CheckForTripleItems(Item pickedItem, int indexOfItem)
        {
            if (indexOfItem > 1 && pickedItem.ItemType == _itemsInSlots[indexOfItem - 1].ItemType &&
                pickedItem.ItemType == _itemsInSlots[indexOfItem - 2].ItemType)
            {
                for (int i = indexOfItem + 1; i < _itemsInSlots.Length; i++)
                {
                    _itemsInSlots[i - 3] = _itemsInSlots[i];
                    _itemsInSlots[i] = null;
                }

                OnTripletCollected?.Invoke(pickedItem, indexOfItem);
            }

            if (_itemsInSlots.Count(x => x == null) == 0)
            {
                OnDefeat?.Invoke();
            }
        }
        
       
        
        public void Dispose()
        {
            _selectorService.OnItemPicked -= HandleItemPicked;
        }
    }
}
