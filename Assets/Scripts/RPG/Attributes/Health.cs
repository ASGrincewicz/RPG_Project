﻿using RPG.Core;
using RPG.Stats;
using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Collider))]
    public class Health : MonoBehaviour, IDamageable, ISaveable
    {
        [SerializeField] private float _health = 100.0f;
        
        private readonly int _dieTrigger = Animator.StringToHash("Die");
        public bool IsDead { get; private set; }

        private void Awake()
        {
            if (TryGetComponent(out BaseStats baseStats))
            {
                _health = baseStats.GetHealth();
            }
        }

        public float GetPercentage()
        {
            if (TryGetComponent(out BaseStats baseStats))
            {
                return 100.0f*(_health / baseStats.GetHealth());
            }

            return 0.0f;
        }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            if (_health == 0 )
            {
                Die();
                print($"{name} is dead.");
            }
            print(_health);
        }

        public void Die()
        {
            if (IsDead) return;
            IsDead = true;
            if(TryGetComponent(out Animator animator))
            {
                animator.SetTrigger(_dieTrigger);
            }

            if(TryGetComponent(out ActionScheduler actionScheduler))
            {
                actionScheduler.CancelAction();
            }
            //Destroy(gameObject,15.0f);
        }
        
        public Vector3 GetPosition() => transform.position;
        public Transform GetTransform() => transform;

        public CapsuleCollider GetCapsuleCollider()
        {
            CapsuleCollider capsuleCollider = GetComponentInParent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                return capsuleCollider;
            } 
            return null;
        }

        public object CaptureState()
        {
            return _health;
        }

        public void RestoreState(object state)
        {
            _health = (float)state;
            if (_health <= 0)
            {
                Die();
            }
        }
    }
}