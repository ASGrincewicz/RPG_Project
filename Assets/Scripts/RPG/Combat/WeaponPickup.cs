using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : Pickup
    {
        [SerializeField] private Weapon _weapon;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.TryGetComponent(out Fighter fighter))
            {
                fighter.EquipWeapon(_weapon);
                _respawnDelay = new WaitForSeconds(_respawnDelayTime);
                StartCoroutine(HideForSeconds());
            }
        }
    }
    
}