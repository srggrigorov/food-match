using UnityEngine;

namespace FoodMatch.Game
{
    public class AudioPlayer : MonoBehaviour
    {
        private static AudioPlayer _instance;
        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
