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
        private ITargetable _target;
        private Animator _animator;
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly int _stopAttackTrigger = Animator.StringToHash("StopAttack");
        private float _timeSinceLastAttack;
        private IDamageable _damageable;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (_target == null) return;
            if (_damageable.IsDead) return;

            if (!GetIsInRange(_target.GetTransform()))
            {
                _mover.MoveTo(_target.GetPosition());
            }
            else
            {
                _mover.Stop();
                AttackBehaviour();
            }
        }

        public void Attack(ITargetable combatTarget)
        {
            _target = combatTarget;
            _damageable = _target.GetTransform().GetComponent<IDamageable>();
            _actionScheduler.StartAction(this);
        }

        public void Cancel()
        {
            _animator.SetTrigger(_stopAttackTrigger);
            _target = null;
            _damageable = null;
            print("Attack canceled.");
        }

        private bool GetIsInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < _weaponRange;
        }

        private void AttackBehaviour()
        {
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _timeBetweenAttacks && !_damageable.IsDead)
            {
                _animator.SetTrigger(_attackTrigger);
                _timeSinceLastAttack = 0f;
                if (_damageable.IsDead)
                {
                    Cancel();
                }
            }
        }
        
        //Animation Event
        private void Hit()
        {
           if (_damageable != null && !_damageable.IsDead)
           {
               _damageable.TakeDamage(_weaponDamage);
           }
        }
    }
}