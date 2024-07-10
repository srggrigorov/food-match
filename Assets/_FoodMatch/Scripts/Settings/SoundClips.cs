using UnityEngine;
namespace FoodMatch.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(SoundClips), fileName = nameof(SoundClips))]
    public class SoundClips : ModuleSettings
    {
        [field: SerializeField]
        public AudioClip PickedItem { get; private set; }
        [field: SerializeField]
        public AudioClip TripletCollected { get; private set; }
        [field: SerializeField]
        public AudioClip Explosion { get; private set; }
    }
}
