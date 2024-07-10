using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace FoodMatch.Game
{
    public class ItemsSpawnerService : IInitializable, IDisposable
    {
        private ItemSpawnGroup _spawnGroup;
        private readonly ItemSpawnGroup.Factory _factory;
        private readonly Transform _spawnPoint;
        private readonly ItemCollectorService _collectorService;
        private readonly Vector3 _randomSpawnBorders = new Vector3(0.5f, 0.6f, 0.2f);

        public ItemsSpawnerService(ItemSpawnGroup.Factory factory, ItemCollectorService collectorService, Transform spawnPoint)
        {
            (_factory, _collectorService, _spawnPoint) = (factory, collectorService, spawnPoint);
            _collectorService.OnItemPicked += HandleItemPicked;
            _collectorService.OnLastItemReturned += HandleItemReturned;
        }

        private void HandleItemPicked(Item item) => _spawnGroup.Items.Remove(item);
        private void HandleItemReturned(Item item) => _spawnGroup.Items.Add(item);

        public void Initialize()
        {
            _spawnGroup = _factory.Create();
            _spawnGroup.transform.position = _spawnPoint.position;
            RandomizeSpawn();
        }

        private void RandomizeSpawn()
        {
            foreach (var item in _spawnGroup.Items)
            {
                Vector3 randomPosition = _spawnPoint.position +
                    new Vector3(Random.Range(-_randomSpawnBorders.x, _randomSpawnBorders.x),
                        Random.Range(-_randomSpawnBorders.y, _randomSpawnBorders.y),
                        Random.Range(-_randomSpawnBorders.z, _randomSpawnBorders.z));

                Quaternion randomRotation = Random.rotation;

                item.Transform.SetPositionAndRotation(randomPosition, randomRotation);
            }
        }

        public void Dispose()
        {
            _collectorService.OnItemPicked -= HandleItemPicked;
            _collectorService.OnLastItemReturned -= HandleItemReturned;
        }

        public Item[] ChooseRandomTriplet()
        {
            var itemsWithTriples = _spawnGroup.Items.Where(
                x => _spawnGroup.Items.Count(y => x.ItemType == y.ItemType) > 2);

            if (!itemsWithTriples.Any())
            {
                return null;
            }

            System.Random random = new System.Random();
            List<Item> listWithTriplets = itemsWithTriples.ToList();
            ItemType randomType = listWithTriplets[random.Next(0, listWithTriplets.Count)].ItemType;
            var itemsWithRandomType = listWithTriplets.Where(x => x.ItemType == randomType).ToList();
            Item[] randomTriplet = itemsWithRandomType.OrderBy(_ => random.Next()).Take(3).ToArray();

            foreach (var item in randomTriplet)
            {
                _spawnGroup.Items.Remove(item);
            }
            return randomTriplet;
        }
    }
}
