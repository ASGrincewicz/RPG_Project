﻿using RPG.Core;
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


        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_weaponPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                
                Instantiate(_weaponPrefab, handTransform);
            }

            if (_animatorOverrideController != null)
            {
                animator.runtimeAnimatorController = _animatorOverrideController;
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, IDamageable target)
        {
            Projectile projectileInstance = Instantiate(Projectile, GetHandTransform(rightHand, leftHand).position,
                Quaternion.identity);
            projectileInstance.transform.position = GetHandTransform(rightHand, leftHand).position;
            projectileInstance.SetTargetAndDamage(target,WeaponDamage);
        }

        public bool HasProjectile()
        {
            return Projectile != null;
        }
        
        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return  IsRightHanded ? rightHand : leftHand;
        }
    }
}