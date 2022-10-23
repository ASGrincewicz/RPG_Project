﻿using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [Header("Weapon Configuration")] 
        [SerializeField] private string _defaultWeaponName = "Unarmed";
        [SerializeField] private Weapon _defaultWeapon = null;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        private Weapon _currentWeapon = null;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private IDamageable _damageable;
        private Animator _animator;
        
        private readonly int _attackTrigger = Animator.StringToHash("Attack");
        private readonly int _stopAttackTrigger = Animator.StringToHash("StopAttack");
        private float _timeSinceLastAttack = Mathf.Infinity;
        
#region Unity Events
        private void Awake()
        {
            TryGetComponent(out _mover);
            TryGetComponent(out _animator);
            TryGetComponent(out _actionScheduler);
            if (_currentWeapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
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
#endregion

#region Public API

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
            _mover.Cancel(); 
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
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            //print($"Current weapon is {weapon.name}");
            weapon.SpawnWeapon(_rightHandTransform,_leftHandTransform ,_animator);
        }

        public IDamageable GetTarget()
        {
            return _damageable;
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
#endregion

#region Private Methods

        private bool GetIsInRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) < _currentWeapon.WeaponRange;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_damageable.GetTransform());
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _currentWeapon.TimeBetweenAttacks && !_damageable.IsDead)
            {
                TriggerAttack();
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
            if (CanAttack(_damageable))
            {
                if (_currentWeapon.HasProjectile())
                {
                    _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _damageable,gameObject);
                }
                else
                {
                    _damageable.TakeDamage(gameObject,_currentWeapon.WeaponDamage);
                }
            }
        }
        //Animation Event
        private void Shoot() => Hit();

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
#endregion
    }
}