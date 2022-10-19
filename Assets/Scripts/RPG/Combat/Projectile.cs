using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _maxLifetime;
        [SerializeField] private float _lifeAfterImpact = 2.0f;
        [SerializeField] private bool _isSeeker;
        [SerializeField] private GameObject _hitEffect;
        [SerializeField] private GameObject[] _destroyOnHit;
        private IDamageable _target;
        private float _damage;

        private void Start()
        {
            if (_target != null)
            {
                transform.LookAt(GetAimLocation());
            }
        }

        private void Update()
        {
            if (_target != null)
            {
                if (_isSeeker && !_target.IsDead)
                { 
                    transform.LookAt(GetAimLocation());
                }
                transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            }
        }

        public void SetTargetAndDamage(IDamageable target, float damage)
        {
            _target = target;
            _damage = damage;
            Destroy(gameObject,_maxLifetime);
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
            if (other.GetComponent<IDamageable>() == _target)
            {
                if (_target.IsDead) return;
                _speed = 0;
                _target.TakeDamage(_damage);
                if (_hitEffect != null)
                {
                    Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
                }

                foreach (GameObject toDestroy in _destroyOnHit)
                {
                    Destroy(gameObject);
                }
            }
            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
