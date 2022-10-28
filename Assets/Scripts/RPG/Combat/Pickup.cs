using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] protected bool _canRespawn = false;
        [SerializeField] protected float _respawnDelayTime = 5.0f;
        protected WaitForSeconds _respawnDelay;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            PickUp(other);
        }

        protected abstract void PickUp(Collider other);


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

        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                controller.TryGetComponent(out Collider other);
                PickUp(other);
            }

            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}