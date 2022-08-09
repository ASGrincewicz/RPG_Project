using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Camera _mainCamera;
        private Health _health;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_health.IsDead) return;
            if(InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hitInfo;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitInfo);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hitInfo.point,1);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                IDamageable target = hit.transform.gameObject.GetComponent<IDamageable>();
                
                if(!_fighter.CanAttack(target)) continue;
                
                if(Input.GetMouseButton(0) && target != this.GetComponent<IDamageable>())
                {
                    //Attack
                    _fighter.Attack(target);
                }

                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
