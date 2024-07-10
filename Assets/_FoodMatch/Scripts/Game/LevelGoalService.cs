using System;
using System.Collections.Generic;
using FoodMatch.Settings;
namespace FoodMatch.Game
{
    public class LevelGoalService : IDisposable
    {
        public event Action AllGoalsAchieved;

        public IReadOnlyDictionary<ItemType, int> ItemsRemainingCounts => _itemsRemainingCounts;

        private readonly ItemCollectorService _collectorService;
        private readonly RandomTripletCollectorService _randomCollectorService;
        private readonly Dictionary<ItemType, int> _itemsRemainingCounts = new Dictionary<ItemType, int>();

        public LevelGoalService(LevelSettings levelSettings,
            ItemCollectorService collectorService,
            RandomTripletCollectorService randomCollectorService)
        {
            (_collectorService, _randomCollectorService) = (collectorService, randomCollectorService);

            foreach (var requirement in levelSettings.Requirements)
            {
                _itemsRemainingCounts.Add(requirement.ItemType, requirement.Count);
            }

            _collectorService.OnTripletCollected += HandleTripletCollected;
            _randomCollectorService.OnRandomTripletCollected += HandleTripletCollected;
        }

        private void HandleTripletCollected(Item lastItem, int index) => HandleTripletCollected(lastItem.ItemType);
        private void HandleTripletCollected(ItemType itemType)
        {
            if (!_itemsRemainingCounts.ContainsKey(itemType))
            {
                return;
            }
            int itemsLeft = _itemsRemainingCounts[itemType] - 3;
            _itemsRemainingCounts[itemType] = itemsLeft > 0 ? itemsLeft : 0;

            if (itemsLeft <= 0)
            {
                _itemsRemainingCounts.Remove(itemType);
            }

            if (_itemsRemainingCounts.Count == 0)
            {
                AllGoalsAchieved?.Invoke();
            }

        }
        public void Dispose()
        {
            _collectorService.OnTripletCollected -= HandleTripletCollected;
        }
    }
}
