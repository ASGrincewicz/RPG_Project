using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class HealthPickup : Pickup
    {
        [SerializeField] private float _amountToHeal;
        protected override void PickUp(Collider other)
        {
            other.TryGetComponent(out Health healthComponent);
            healthComponent.Heal(_amountToHeal);
            _respawnDelay = new WaitForSeconds(_respawnDelayTime);
            StartCoroutine(HideForSeconds());
        }
    }
}