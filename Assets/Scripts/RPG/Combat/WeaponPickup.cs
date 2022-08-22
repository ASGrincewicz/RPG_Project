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
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}