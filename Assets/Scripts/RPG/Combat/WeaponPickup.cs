using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : Pickup
    {
        [SerializeField] private WeaponConfig weaponConfig;
        
        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PickUp(other);
        }

        protected override void PickUp(Collider other)
        {
            if (other.TryGetComponent(out Fighter fighter))
            {
                fighter.EquipWeapon(weaponConfig);
                _respawnDelay = new WaitForSeconds(_respawnDelayTime);
                StartCoroutine(HideForSeconds());
            }
        }
    }
    
}