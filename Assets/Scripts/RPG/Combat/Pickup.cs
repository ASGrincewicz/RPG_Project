using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public abstract class Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] protected bool _canRespawn = false;
        [SerializeField] protected float _respawnDelayTime = 5.0f;
        [SerializeField] protected float _grabDistance = 5.0f;
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
            float distance = Vector3.Distance(controller.transform.position, gameObject.transform.position);
            if ( distance <= _grabDistance)
            {
                return true;
            }
            return false;
        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}