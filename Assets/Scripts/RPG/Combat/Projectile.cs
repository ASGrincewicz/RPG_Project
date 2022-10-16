using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private IDamageable _target;
        private float _damage;
        
        private void Update()
        {
            if (_target != null)
            {
                transform.LookAt(GetAimLocation());
                transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            }
        }

        public void SetTargetAndDamage(IDamageable target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = _target.GetCapsuleCollider();
            if (targetCollider != null)
            {
                return _target.GetPosition() + Vector3.up * targetCollider.height / 1.5f;
            }
            else
            {
                return _target.GetPosition();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<IDamageable>() != null)
            {
                _target.TakeDamage(_damage);
                Destroy(gameObject);
            }
            else
            {
                return;
            }
        }
    }
}
