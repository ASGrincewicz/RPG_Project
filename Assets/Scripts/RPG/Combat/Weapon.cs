using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private GameObject _weaponPrefab;
        
        [field: SerializeField] public float WeaponDamage;
        [field: SerializeField] public float WeaponRange;
        [field: SerializeField] public float TimeBetweenAttacks;
        [field: SerializeField] public bool IsRightHanded = true;
        [field: SerializeField] public Projectile Projectile;

        private const string _weaponName = "Weapon";

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);
            if (_weaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                
                GameObject weapon = Instantiate(_weaponPrefab, handTransform);
                weapon.name = _weaponName;
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
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}