using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FoodMatch.Game
{
    public class Item : MonoBehaviour
    {
        public bool IsInteractable { get; private set; } = true;

        [field: SerializeField]
        public ItemType ItemType { get; private set; }
        [field: SerializeField]
        public Transform Transform { get; private set; }
        [field: SerializeField]
        public Rigidbody Rigidbody { get; private set; }

        [SerializeField]
        private Collider[] _colliders;

        [Header("Components for destroying")]
        [SerializeField]
        protected Renderer _renderer;
        [SerializeField]
        protected ParticleSystem _particleSystem;


        [Header("Selection")]
        [SerializeField]
        private Outline _outline;
        [SerializeField]
        private float _scaleModifier;
        [SerializeField]
        private float _scaleChangeDuration;

        public void ToggleSelected(bool value)
        {
            DOTween.Kill(this);
            _outline.enabled = value;
            Vector3 targetScale = Vector3.one * (value ? _scaleModifier : 1);
            Transform.DOScale(targetScale, _scaleChangeDuration);
        }

        public void SetInteractable(bool value)
        {
            if (IsInteractable == value)
            {
                return;
            }
            IsInteractable = value;
            foreach (var itemCollider in _colliders)
            {
                itemCollider.enabled = IsInteractable;
            }
            Rigidbody.isKinematic = !IsInteractable;
        }

        public void DestroyWithParticles()
        {
            _renderer.enabled = false;
            _particleSystem.Play();
            DOTween.Kill(this);
            Destroy(gameObject, _particleSystem.main.duration);
        }
    }
}
