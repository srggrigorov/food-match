using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace FoodMatch.Menu
{
    public class LevelLoaderButton : MonoBehaviour
    {
        [SerializeField]
        private string _levelId;
        [SerializeField]
        private Button _button;

        [Inject]
        private void Construct(LevelLoaderService levelLoaderService)
        {
            _button.onClick.AddListener(() => levelLoaderService.LoadLevelAsync(_levelId).Forget());
        }
    }
}
