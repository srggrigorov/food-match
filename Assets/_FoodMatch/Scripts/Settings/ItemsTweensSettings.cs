using UnityEngine;
namespace FoodMatch.Settings

{
    [CreateAssetMenu(menuName = "Settings/" + nameof(ItemsTweensSettings), fileName = nameof(ItemsTweensSettings))]
    public class ItemsTweensSettings : ModuleSettings
    {
        [field: SerializeField]
        public float ItemJumpForce { get; private set; }
        [field: SerializeField]
        public float MoveToSlotDuration { get; private set; }
        [field: SerializeField]
        public float ClearTripleHeight { get; private set; }
    }
}
