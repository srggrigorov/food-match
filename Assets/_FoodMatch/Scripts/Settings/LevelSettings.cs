using System.Collections.Generic;
using FoodMatch.Game;
using UnityEngine;

namespace FoodMatch.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(LevelSettings), fileName = nameof(LevelSettings))]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public int TimeSeconds { get; private set; }
        [field: SerializeField]
        public ItemSpawnGroup SpawnGroup { get; private set; }
        [field: SerializeField]
        public List<LevelRequirement> Requirements { get; private set; }
    }
}
