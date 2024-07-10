using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FoodMatch.Game
{
    public class Dynamite : Item
    {
        [Header("Explosion")]
        [SerializeField]
        private float _radius;
        [SerializeField]
        private float _force;
        [SerializeField]
        private ParticleSystem _explosion;
        [SerializeField]
        private LayerMask _layerMask;

        public void Explode()
        {
            Collider[] overlappedColliders = new Collider[100];
            Physics.OverlapSphereNonAlloc(Transform.position, _radius, overlappedColliders, _layerMask);

            HashSet<Rigidbody> itemRigidbodies = new HashSet<Rigidbody>();
            foreach (var itemCollider in overlappedColliders)
            {
                if (itemCollider == null || itemRigidbodies.Contains(itemCollider.attachedRigidbody)) continue;
                Rigidbody attachedRigidbody;
                (attachedRigidbody = itemCollider.attachedRigidbody).AddExplosionForce(_force, Transform.position, _radius, 1.5f);
                itemRigidbodies.Add(attachedRigidbody);
            }

            itemRigidbodies.Clear();
            _renderer.enabled = false;
            DOTween.Kill(this);
            _explosion.Play();
            Destroy(gameObject, _explosion.main.duration);
        }
    }
}
