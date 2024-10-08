﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FoodMatch.Settings
{
    [CreateAssetMenu(fileName = nameof(AssetsSettings), menuName = "Settings/" + nameof(AssetsSettings), order = 999)]
    public class AssetsSettings : ScriptableObject
    {
        public List<AssetReferenceT<ModuleSettings>> settingsList;
    }
}
