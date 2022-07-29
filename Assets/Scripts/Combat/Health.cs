using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour, IDamageable, ITargetable
    {
        [SerializeField] private float _health = 100.0f;
        //private Animator _animator;
        private readonly int _dieTrigger = Animator.StringToHash("Die");
        public bool IsDead { get; private set; }
        
        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            print(_health);
            if (_health == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (IsDead) return;
            GetComponent<Animator>().SetTrigger(_dieTrigger);
            IsDead = true;
            Destroy(gameObject,5.0f);
        }
        
        public Vector3 GetPosition() => transform.position;
        public Transform GetTransform() => transform;

    }
}