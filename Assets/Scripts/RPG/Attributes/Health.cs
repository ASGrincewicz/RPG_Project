using RPG.Core;
using RPG.Stats;
using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    [RequireComponent(typeof(Collider))]
    public class Health : MonoBehaviour, IDamageable, ISaveable
    {
        [field: SerializeField] public float HealthPoints { get; private set; } = -1f;
        [SerializeField] private float _regenerationPercentage = 75.0f;
        private BaseStats _baseStats;
        
        private readonly int _dieTrigger = Animator.StringToHash("Die");
        public bool IsDead { get; private set; }

        private void Awake()
        {
            if(HealthPoints <= 0)
            {
                if (TryGetComponent(out _baseStats))
                {
                    _baseStats.onLevelUp += RegenerateHealth;
                    HealthPoints = _baseStats.GetStat(Stat.Health);
                }
            }
        }

        public float GetMaxHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            if (_baseStats != null)
            {
                return 100.0f*(HealthPoints / _baseStats.GetStat(Stat.Health));
            }

            return 0.0f;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print($"{gameObject.name} took damage: {damage}.");
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            if (HealthPoints == 0 )
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
            float xpReward = _baseStats.GetStat(Stat.XPReward);
            if (instigator.TryGetComponent(out Experience experience))
            {
                experience.GainXP(xpReward);
            }
        }

        private void RegenerateHealth()
        {
            if (_baseStats == null) return;
            
            float regenHP = _baseStats.GetStat(Stat.Health) * (_regenerationPercentage/100.0f);
            HealthPoints = Mathf.Max(HealthPoints, regenHP);
            
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
            return HealthPoints;
        }

        public void RestoreState(object state)
        {
            HealthPoints = (float)state;
            if (HealthPoints <= 0)
            {
                Die();
            }
        }
    }
}