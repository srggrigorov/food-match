using System;
using UnityEngine;

namespace FoodMatch.Game
{
    [Serializable]
    public class LevelRequirement
    {
        [field: SerializeField]
        public ItemType ItemType { get; private set; }
        [field: SerializeField]
        public int Count { get; private set; }
    }
}
