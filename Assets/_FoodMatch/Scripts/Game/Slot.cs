using UnityEngine;
namespace FoodMatch.Game
{
    public class Slot : MonoBehaviour
    {
        [HideInInspector]
        public Item Item;

        [field: SerializeField]
        public Transform ItemContainer { get; private set; }
    }
}
