using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private GameObject _weaponPrefab = null;
        
        [field: SerializeField] public float WeaponDamage;
        [field: SerializeField] public float WeaponRange;
        [field: SerializeField] public float TimeBetweenAttacks;
        

        public void SpawnWeapon(Transform handTransform, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                Instantiate(_weaponPrefab, handTransform);
            }

            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
        }
    }
}