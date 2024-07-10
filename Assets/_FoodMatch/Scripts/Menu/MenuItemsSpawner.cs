using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FoodMatch.Menu
{
    public class MenuItemsSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _spawnBordersX;
        [SerializeField]
        private List<Rigidbody> _items;
        [SerializeField]
        private float _deactivationDelay;
        [SerializeField]
        private float _spawnDelay;
        [SerializeField]
        private float _itemStartVelocity;
        [SerializeField]
        private float _itemMaxAngularVelocity;

        private Transform _transform;
        private WaitForSeconds _waitForSpawnDelay;
        private WaitForSeconds _waitForDeactivationDelay;

        private void Awake()
        {
            _transform = transform;
            _waitForSpawnDelay = new WaitForSeconds(_spawnDelay);
            _waitForDeactivationDelay = new WaitForSeconds(_deactivationDelay);
            StartCoroutine(SpawnItems());
        }

        private IEnumerator SpawnItems()
        {
            var randomSelection = _items.Where(x => !x.gameObject.activeInHierarchy);
            Rigidbody item = randomSelection.ElementAtOrDefault(Random.Range(0, randomSelection.Count()));
            item.gameObject.SetActive(true);
            item.position = _transform.position + Vector3.right * Random.Range(_spawnBordersX.x, _spawnBordersX.y);
            item.velocity = Vector3.down * _itemStartVelocity;
            item.angularVelocity = new Vector3(
                Random.Range(-_itemMaxAngularVelocity, _itemMaxAngularVelocity),
                Random.Range(-_itemMaxAngularVelocity, _itemMaxAngularVelocity),
                Random.Range(-_itemMaxAngularVelocity, _itemMaxAngularVelocity));

            StartCoroutine(DeactivateItem(item));

            yield return _waitForSpawnDelay;
            StartCoroutine(SpawnItems());
        }

        IEnumerator DeactivateItem(Rigidbody item)
        {
            yield return _waitForDeactivationDelay;
            item.gameObject.SetActive(false);
        }
    }
}
