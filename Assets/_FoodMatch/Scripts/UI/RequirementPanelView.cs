using FoodMatch.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FoodMatch.UI
{
    public class RequirementPanelView : MonoBehaviour
    {
        public ItemType ItemType { get; private set; }

        [field: SerializeField]
        public RectTransform RectTransform { get; private set; }
        [field: SerializeField]
        public Image ItemIcon { get; private set; }
        [field: SerializeField]
        public TMP_Text CountText { get; private set; }
        
        [SerializeField]
        private float _punchScaleModifier;
        [SerializeField]
        private float _punchScaleDuration;

        public void Initialize(ItemType type, Sprite iconSprite, int count)
        {
            ItemType = type;
            ItemIcon.sprite = iconSprite;
            ItemIcon.enabled = true;
            CountText.text = count.ToString();
        }

        public void SetCountText(int count)
        {
            CountText.text = count.ToString();
        }

        public class Factory : PlaceholderFactory<RequirementPanelView>
        {
            public RequirementPanelView Create(Transform parent)
            {
                var view = Create();
                view.transform.SetParent(parent);
                return view;
            }
        }
    }
}
