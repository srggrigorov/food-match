using System.Collections.Generic;
using UnityEngine;
namespace FoodMatch.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(LevelsSettings), fileName = nameof(LevelsSettings))]
    public class LevelsSettings : ModuleSettings
    {
        [field: SerializeField]
        public List<LevelSettings> SettingsList { get; private set; }
    }
}
