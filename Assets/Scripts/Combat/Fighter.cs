using System;
using RPG.Core;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2.0f;
        [SerializeField] private float _timeBetweenAttacks;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Transform _target;
        private Animator _animator;
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private float _timeSinceLastAttack;

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
            
            if (!GetIsInRange(_target))
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Stop();
                AttackBehaviour();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            _target = combatTarget.transform;
            _actionScheduler.StartAction(this);
        }

        public void Cancel()
        {
            _target = null;
            print("Attack canceled.");
        }

        private bool GetIsInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < _weaponRange;
        }

        private void AttackBehaviour()
        {
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                _animator.SetTrigger(_attackTrigger);
                _timeSinceLastAttack = 0f;
            }
            
        }
        
        //Animation Event
        private void Hit()
        {
            print("Enemy hit.");
        }
    }
}