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
        
        private readonly int _dieTrigger = Animator.StringToHash("Die");
        public bool IsDead { get; private set; }

        private void Awake()
        {
            if(HealthPoints <= 0)
            {
                if (TryGetComponent(out BaseStats baseStats))
                {
                    baseStats.onLevelUp += RegenerateHealth;
                    HealthPoints = baseStats.GetStat(Stat.Health);
                }
            }
        }

        public float GetPercentage()
        {
            if (TryGetComponent(out BaseStats baseStats))
            {
                return 100.0f*(HealthPoints / baseStats.GetStat(Stat.Health));
            }

            return 0.0f;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);
            if (HealthPoints == 0 )
            {
                Die();
                AwardExperience(instigator);
                //print($"{name} is dead.");
            }
            //print(HealthPoints);
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

        private void AwardExperience(GameObject instigator)
        {
            if (!TryGetComponent(out BaseStats baseStats)) return;
            float xpReward = baseStats.GetStat(Stat.XPReward);
            if (instigator.TryGetComponent(out Experience experience))
            {
                experience.GainXP(xpReward);
            }
        }

        private void RegenerateHealth()
        {
            if (TryGetComponent(out BaseStats baseStats))
            {
                float regenHP = baseStats.GetStat(Stat.Health) * (_regenerationPercentage/100.0f);
                HealthPoints = Mathf.Max(HealthPoints, regenHP);
            } 
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