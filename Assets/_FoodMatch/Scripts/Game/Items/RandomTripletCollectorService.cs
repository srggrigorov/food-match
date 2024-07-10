using System;
using DG.Tweening;
using UnityEngine;

namespace FoodMatch.Game
{
    public class RandomTripletCollectorService
    {
        public event Action<ItemType> OnRandomTripletCollected;

        private readonly ItemsSpawnerService _spawnerService;
        private readonly SoundService _soundService;

        public RandomTripletCollectorService(ItemsSpawnerService spawnerService, SoundService soundService)
        {
            (_spawnerService, _soundService) = (spawnerService, soundService);
        }

        public void CollectRandomTriplet()
        {
            var triplet = _spawnerService.ChooseRandomTriplet();
            if (triplet == null)
            {
                return;
            }
            ItemType itemType = triplet[0].ItemType;
            Sequence sequence = DOTween.Sequence();
            foreach (var item in triplet)
            {
                item.SetInteractable(false);
                item.ToggleSelected(true);
                sequence.Join(item.Transform.DOMove(Vector3.up, 0.5f).OnComplete(item.DestroyWithParticles));
            }
            sequence.Play().OnComplete(() => _soundService.PlaySoundOnce(_soundService.SoundClips.TripletCollected));
            OnRandomTripletCollected?.Invoke(itemType);
        }
    }
}
