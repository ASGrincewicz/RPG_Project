using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Camera _mainCamera;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
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
                    _mover.StartMoveAction(hitInfo.point);
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
                CombatTarget target = hit.transform.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                {
                    continue;
                }
                if(Input.GetMouseButtonDown(0))
                {
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
