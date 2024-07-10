using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FoodMatch.Game
{
    public class ItemSpawnGroup : MonoBehaviour
    {
        [field: SerializeField]
        public List<Item> Items;

        public class Factory : PlaceholderFactory<ItemSpawnGroup>
        {

        }
    }
}
