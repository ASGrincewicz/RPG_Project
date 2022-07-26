﻿using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Stats;
using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Collider))]
    public class Health : MonoBehaviour, IDamageable, ISaveable
    {
        [field: SerializeField] public LazyValue<float> HealthPoints { get; private set; }
        [field:SerializeField] public bool IsDead { get; private set; }
        [SerializeField] private float _regenerationPercentage = 75.0f;
        private BaseStats _baseStats;
        private float _state = 0;
        private readonly int _dieTrigger = Animator.StringToHash("Die");
       

        private void Awake()
        {
            TryGetComponent(out _baseStats);
            HealthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            if (_baseStats != null)
            {
                return _baseStats.GetStat(Stat.Health);
            }

            return 0;
        }
        private void OnEnable()
        {
            if (TryGetComponent(out _baseStats))
            {
                _baseStats.onLevelUp += RegenerateHealth;
            }
        }

        private void OnDisable()
        {
            if (TryGetComponent(out _baseStats))
            {
                _baseStats.onLevelUp -= RegenerateHealth;
            }
        }

        private void Start()
        {
            HealthPoints.ForceInit();
            /*if (_baseStats != null && _state == 0 && !IsDead)
            {
                HealthPoints.value = _baseStats.GetStat(Stat.Health);                                                                         
            }
            else if(HealthPoints.value <= 0 ||_baseStats == null || IsDead)
            {
                HealthPoints.value = 0;
            }*/
        }

        public float GetMaxHealth()
        {
            if (_baseStats == null) return 0;
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            if (_baseStats != null)
            {
                return 100.0f*(HealthPoints.value / _baseStats.GetStat(Stat.Health));
            }

            return 0.0f;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print($"{gameObject.name} took damage: {damage:0.00}.");
            HealthPoints.value = Mathf.Max(HealthPoints.value - damage, 0);
            if (HealthPoints.value == 0 )
            {
                Die();
                AwardExperience(instigator);
            }
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
        }

        private void AwardExperience(GameObject instigator)
        {
            if (_baseStats == null) return;
            
            if (instigator.TryGetComponent(out Experience experience))
            {
                float xpReward = _baseStats.GetStat(Stat.XPReward);
                experience.GainXP(xpReward);
            }
        }

        private void RegenerateHealth()
        {
            if (_baseStats == null) return;
            
            float regenHP = _baseStats.GetStat(Stat.Health) * (_regenerationPercentage/100.0f);
            HealthPoints.value = Mathf.Max(HealthPoints.value, regenHP);
            
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
            return HealthPoints.value;
        }

        public void RestoreState(object state)
        {
            _state = (float)state;
            HealthPoints.value = (float)state;
            if (HealthPoints.value <= 0)
            {
                Die();
            }
        }
    }
}