using System;
using System.Collections.Generic;
using FoodMatch.Game;
using UnityEngine;

namespace FoodMatch.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(ItemsIcons), fileName = nameof(ItemsIcons))]
    public class ItemsIcons : ModuleSettings
    {
        [SerializeField]
        private List<ItemIcon> _icons = new List<ItemIcon>();

        public IReadOnlyList<ItemIcon> Icons => _icons;
    }

    [Serializable]
    public sealed class ItemIcon
    {
        [field: SerializeField]
        public ItemType ItemType { get; private set; }
        [field: SerializeField]
        public Sprite Sprite { get; private set; }
    }
}
