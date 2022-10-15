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
        

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                Transform handTransform = IsRightHanded ? rightHand : leftHand;
                
                Instantiate(_weaponPrefab, handTransform);
            }

            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
        }
    }
}