using System;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponDamage = 5.0f;
        [SerializeField] private float _weaponRange = 2.0f;
        [SerializeField] private float _timeBetweenAttacks;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private IDamageable _damageable;
        private Animator _animator;
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly int _stopAttackTrigger = Animator.StringToHash("StopAttack");
        private float _timeSinceLastAttack = Mathf.Infinity;
       

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_damageable == null) return;
            if (_damageable.IsDead) return;

            if (!GetIsInRange(_damageable.GetTransform()))
            {
                _mover.MoveTo(_damageable.GetPosition(),1f);
            }
            else
            {
                _mover.Stop();
                AttackBehaviour();
            }
        }

        public void Attack(IDamageable combatTarget)
        {
            _damageable = combatTarget;
           // _damageable = _target.GetTransform().GetComponent<IDamageable>();
            _actionScheduler.StartAction(this);
        }

        public void Cancel()
        {
            StopAttack();
            _damageable = null;
            print("Attack canceled.");
        }

        private bool GetIsInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < _weaponRange;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_damageable.GetTransform());
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _timeBetweenAttacks && !_damageable.IsDead)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
                if (_damageable.IsDead)
                {
                    Cancel();
                }
            }
        }

        public bool CanAttack(IDamageable target)
        {
            if (target == null)
            {
                return false;
            }
            //Since this line will be reached only if target isn't null, only need to check if dead.
            return !target.IsDead;
        }
        
        //Animation Event
        private void Hit()
        {
           if (CanAttack(_damageable))
           {
               _damageable.TakeDamage(_weaponDamage);
           }
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(_stopAttackTrigger);
            _animator.SetTrigger(_attackTrigger);
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(_attackTrigger);
            _animator.SetTrigger(_stopAttackTrigger);
        }
    }
}