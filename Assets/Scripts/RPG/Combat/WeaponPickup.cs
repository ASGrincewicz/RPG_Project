using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : Pickup
    {
        [SerializeField] private Weapon _weapon;
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out Fighter fighter))
                {
                    fighter.EquipWeapon(_weapon);
                    Destroy(gameObject);
                }
            }
        }
    }
}