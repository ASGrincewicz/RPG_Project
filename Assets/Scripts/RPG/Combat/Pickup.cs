using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] protected bool _canRespawn = false;
        [SerializeField] protected float _respawnDelayTime = 5.0f;
        protected WaitForSeconds _respawnDelay;
        protected abstract void OnTriggerEnter(Collider other);

        protected virtual IEnumerator HideForSeconds()
        {
            ShowPickup(false);
            yield return _respawnDelay;
            ShowPickup(true);
        }

        protected virtual void ShowPickup(bool shouldShow)
        {
            TryGetComponent(out Collider pickupCollider);
            pickupCollider.enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}