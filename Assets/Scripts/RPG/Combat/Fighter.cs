using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Stats;
using Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [Header("Weapon Configuration")] 
        [SerializeField] private string _defaultWeaponName = "Unarmed";
        [SerializeField] private Weapon _defaultWeapon = null;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        private LazyValue<Weapon> _currentWeapon;
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
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            TryGetComponent(out _mover);
            TryGetComponent(out _animator);
            TryGetComponent(out _actionScheduler);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }
        private void Start()
        {
           _currentWeapon.ForceInit();
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
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            weapon.SpawnWeapon(_rightHandTransform,_leftHandTransform ,_animator);
        }

        public IDamageable GetTarget()
        {
            return _damageable;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.PercentageBonus;
            }
        }

        public object CaptureState()
        {
            if (_currentWeapon != null)
            {
                return _currentWeapon.value.name;
            }

            return _defaultWeapon.name;
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
            return Vector3.Distance(transform.position, target.position) < _currentWeapon.value.WeaponRange;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_damageable.GetTransform());
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _currentWeapon.value.TimeBetweenAttacks && !_damageable.IsDead)
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
                TryGetComponent(out BaseStats baseStats);
                float damage = baseStats.GetStat(Stat.Damage);
                if (_currentWeapon.value.HasProjectile())
                {
                    _currentWeapon.value.LaunchProjectile(_rightHandTransform, _leftHandTransform, _damageable,gameObject,damage);
                }
                else
                {
                   
                    _damageable.TakeDamage(gameObject,damage);
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