using FoodMatch.Game;
using FoodMatch.Input;
using FoodMatch.UI;
using UnityEngine;
using Zenject;

namespace FoodMatch.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Camera _levelCamera;
        [SerializeField]
        private Slot[] _slots;
        [SerializeField]
        private EndgameScreenView _endgameScreen;
        [SerializeField]
        private Transform _itemsSpawnPoint;

        [Header("Requirements UI")]
        [SerializeField]
        private RequirementPanelView _requirementPanelViewPrefab;
        [SerializeField]
        private RectTransform _panelsContainer;
        [SerializeField]
        private RectTransform _anchorsContainer;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ItemSelectorService>().AsSingle().WithArguments(_levelCamera).NonLazy();
            Container.BindInterfacesAndSelfTo<ItemCollectorService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ItemsSpawnerService>().AsSingle().WithArguments(_itemsSpawnPoint).NonLazy();
            Container.BindInterfacesAndSelfTo<RandomTripletCollectorService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SlotsViewService>().AsSingle().WithArguments(_slots,_itemsSpawnPoint).NonLazy();
            Container.BindInterfacesAndSelfTo<LevelGoalService>().AsSingle().NonLazy();

            Container.BindFactory<RequirementPanelView, RequirementPanelView.Factory>()
                .FromComponentInNewPrefab(_requirementPanelViewPrefab).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<RequirementsViewController>().AsSingle().WithArguments(_panelsContainer, _anchorsContainer).NonLazy();
            Container.BindInterfacesAndSelfTo<TimerService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EndgameService>().AsSingle().WithArguments(_endgameScreen).NonLazy();
        }
    }
}
