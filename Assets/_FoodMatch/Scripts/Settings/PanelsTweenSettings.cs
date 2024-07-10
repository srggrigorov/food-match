using UnityEngine;
namespace FoodMatch.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(PanelsTweenSettings), fileName = nameof(PanelsTweenSettings))]
    public class PanelsTweenSettings : ModuleSettings
    {
        [field: SerializeField]
        public float HideDuration { get; private set; }
        [field: SerializeField]
        public float PunchForceModifier { get; private set; }
    }
}
