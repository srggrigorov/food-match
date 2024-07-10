using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FoodMatch.Game;
using FoodMatch.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace FoodMatch.UI
{
    public class RequirementsViewController : IDisposable, IInitializable
    {
        private readonly LevelGoalService _goalService;
        private readonly SlotsViewService _slotsViewService;
        private readonly RandomTripletCollectorService _randomCollectorService;
        private readonly RequirementPanelView.Factory _panelViewFactory;
        private readonly RectTransform _panelsContainer;
        private readonly RectTransform _anchorsContainer;
        private readonly ItemsIcons _itemsIcons;
        private readonly PanelsTweenSettings _tweenSettings;
        private readonly Dictionary<ItemType, RequirementPanelView> _panelViews = new Dictionary<ItemType, RequirementPanelView>();
        private readonly Dictionary<ItemType, RectTransform> _panelsAnchors = new Dictionary<ItemType, RectTransform>();

        public RequirementsViewController(LevelGoalService goalService, SlotsViewService slotsViewService, AssetsManager assetsManager,
            RequirementPanelView.Factory panelViewFactory, RectTransform panelsContainer, RectTransform anchorsContainer,
            RandomTripletCollectorService randomCollectorService)
        {
            (_goalService, _slotsViewService, _panelViewFactory) = (goalService, slotsViewService, panelViewFactory);
            (_panelsContainer, _anchorsContainer, _randomCollectorService) = (panelsContainer, anchorsContainer, randomCollectorService);

            _itemsIcons = assetsManager.GetModuleSettings<ItemsIcons>();
            _tweenSettings = assetsManager.GetModuleSettings<PanelsTweenSettings>();

            _slotsViewService.OnTripletCleared += UpdateItemCountText;
            _randomCollectorService.OnRandomTripletCollected += UpdateItemCountText;
            foreach (var requirement in _goalService.ItemsRemainingCounts)
            {
                CreatePanel(requirement.Key, requirement.Value);
                CreatePanelAnchor(requirement.Key, _panelViews[requirement.Key]);
            }
        }
        async private void UpdateItemCountText(ItemType itemType)
        {
            if (!_panelViews.ContainsKey(itemType))
            {
                return;
            }
            RequirementPanelView panel = _panelViews[itemType];
            var sequence = DOTween.Sequence().Append(panel.RectTransform
                .DOPunchScale(Vector3.one * _tweenSettings.PunchForceModifier, _tweenSettings.HideDuration, 0, 0));

            bool noItemsLeft = !_goalService.ItemsRemainingCounts.ContainsKey(itemType);
            panel.SetCountText(noItemsLeft ? 0 : _goalService.ItemsRemainingCounts[itemType]);

            if (!noItemsLeft)
            {
                return;
            }

            panel.RectTransform.DOScale(Vector3.zero, _tweenSettings.HideDuration);
            RectTransform anchor = _panelsAnchors[itemType];
            _panelsAnchors.Remove(itemType);
            Object.Destroy(anchor.gameObject);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_anchorsContainer);
            await UniTask.NextFrame();
            foreach (var key in _panelsAnchors.Keys)
            {
                _panelViews[key].RectTransform.DOMove(_panelsAnchors[key].transform.position, _tweenSettings.HideDuration);
            }
            sequence.OnComplete(() =>
                {
                    _panelViews.Remove(itemType);
                    Object.Destroy(panel.gameObject);
                });
        }

        private void CreatePanelAnchor(ItemType itemType, RequirementPanelView panel)
        {
            RectTransform anchor = (RectTransform)new GameObject($"{itemType.ToString()}Anchor",
                typeof(RectTransform)).transform;

            anchor.SetParent(_anchorsContainer);
            anchor.sizeDelta = panel.RectTransform.sizeDelta;
            _panelsAnchors.Add(itemType, anchor);
        }

        private void CreatePanel(ItemType itemType, int itemsCount)
        {
            RequirementPanelView panelView = _panelViewFactory.Create(_panelsContainer);
            Sprite iconSprite = _itemsIcons.Icons.First(x => x.ItemType == itemType).Sprite;
            panelView.Initialize(itemType, iconSprite, itemsCount);
            _panelViews.Add(itemType, panelView);
        }

        public void Dispose()
        {
            _slotsViewService.OnTripletCleared -= UpdateItemCountText;
            _randomCollectorService.OnRandomTripletCollected -= UpdateItemCountText;
        }

        public void Initialize()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_anchorsContainer);
            foreach (var key in _panelsAnchors.Keys)
            {
                _panelViews[key].RectTransform.position = _panelsAnchors[key].position;
                _panelViews[key].RectTransform.DOScale(Vector3.one, _tweenSettings.HideDuration);
            }
        }
    }
}
