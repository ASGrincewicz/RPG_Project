using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Stats;
using Saving;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [Header("Weapon Configuration")] 
        [SerializeField] private string _defaultWeaponName = "Unarmed";
        [SerializeField] private WeaponConfig _defaultWeaponConfig = null;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        
        private WeaponConfig _currentWeaponConfig;
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
            if (_defaultWeaponConfig == null)
            {
                Debug.LogError($"No Default Weapon Assigned for {gameObject.name}!!");
            }
            _currentWeaponConfig = _defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            TryGetComponent(out _mover);
            TryGetComponent(out _animator);
            TryGetComponent(out _actionScheduler);
        }

        private Weapon SetupDefaultWeapon()
        {
           return  AttachWeapon(_defaultWeaponConfig);
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

            if (!GetIsInRange(_damageable.GetPosition()))
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
            if (target == null || target == GetComponent<IDamageable>()) return false;
            
            if (!_mover.CanMoveTo(target.GetPosition())) return false;
            //Since this line will be reached only if target isn't null, only need to check if dead.
            return !target.IsDead;
        }
        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeaponConfig = weaponConfig;
            _currentWeapon.value = AttachWeapon(weaponConfig);
        }

        public bool GetIsInRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) < _currentWeaponConfig.WeaponRange;
        }

        public IDamageable GetTarget()
        {
            return _damageable;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.PercentageBonus;
            }
        }

        public object CaptureState()
        {
            if (_currentWeaponConfig != null)
            {
                return _currentWeaponConfig.name;
            }

            return _defaultWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weaponConfig = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }
#endregion

#region Private Methods

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.SpawnWeapon(_rightHandTransform,_leftHandTransform ,_animator);
        }
       private void AttackBehaviour()
        {
            transform.LookAt(_damageable.GetTransform());
            //Throttle Attack Animation
            if (_timeSinceLastAttack > _currentWeaponConfig.TimeBetweenAttacks && !_damageable.IsDead)
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
                if (_currentWeapon.value != null)
                {
                    _currentWeapon.value.OnHit();
                }
                    
                if (_currentWeaponConfig.HasProjectile())
                {
                    _currentWeaponConfig.LaunchProjectile(_rightHandTransform, _leftHandTransform, _damageable,gameObject,damage);
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