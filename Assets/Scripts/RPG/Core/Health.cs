using System;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(Collider))]
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health = 100.0f;
        //private Animator _animator;
        private readonly int _dieTrigger = Animator.StringToHash("Die");
        public bool IsDead { get; private set; }

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
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(_dieTrigger);
            }

            ActionScheduler actionScheduler = GetComponent<ActionScheduler>();
            if (actionScheduler != null)
            {
                actionScheduler.CancelAction();
            }
            Destroy(gameObject,15.0f);
        }
        
        public Vector3 GetPosition() => transform.position;
        public Transform GetTransform() => transform;

    }
}