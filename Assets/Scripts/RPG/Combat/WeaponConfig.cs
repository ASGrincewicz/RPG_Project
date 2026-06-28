using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/Make New Weapon Config", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private Weapon _weaponPrefab;
        
        [field: SerializeField] public float WeaponDamage;
        [field: SerializeField] public float PercentageBonus = 0.0f;
        [field: SerializeField] public float WeaponRange;
        [field: SerializeField] public float TimeBetweenAttacks;
        [field: SerializeField] public bool IsRightHanded = true;
        [field: SerializeField] public Projectile Projectile;

        private const string _weaponName = "Weapon";

        public Weapon SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);

            Weapon weapon = null;
            if (_weaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                
                weapon = Instantiate(_weaponPrefab, handTransform);
                weapon.gameObject.name = _weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, IDamageable target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(Projectile, GetHandTransform(rightHand, leftHand).position,
                Quaternion.identity);
            projectileInstance.transform.position = GetHandTransform(rightHand, leftHand).position;
            projectileInstance.SetTargetAndDamage(target,instigator,calculatedDamage);
        }

        public bool HasProjectile()
        {
            return Projectile != null;
        }
        
        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return  IsRightHanded ? rightHand : leftHand;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(_weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(_weaponName);
            }

            if (oldWeapon == null) return;
            oldWeapon.gameObject.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}