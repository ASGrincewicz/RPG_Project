using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Camera _mainCamera;
        private Health _health;
        private IDamageable _damageable;

        private void Awake()
        {
            if(!TryGetComponent(out _fighter))
            {
                Debug.LogError("Fighter behaviour not assigned.");
            }
            
            if(!TryGetComponent(out _mover))
            {
                Debug.LogError("Mover behaviour not assigned.");
            }
           
            if (!TryGetComponent(out _health))
            {
                Debug.LogError("Health not found!");
            }
            if (!TryGetComponent(out _damageable))
            {
                Debug.LogError("IDamageable interface not found!");
            }
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            try
            {
                if (_mover.TryGetComponent(out NavMeshAgent navMeshAgent))
                {
                    navMeshAgent.enabled = true;
                }
            }
            catch
            {
                Debug.LogError("Nav Mesh Agent not found!");
            }
        }

        private void OnDisable()
        {
            try
            {
                if (_mover.TryGetComponent(out NavMeshAgent navMeshAgent))
                {
                    navMeshAgent.enabled = false;
                }
            }
            catch
            {
                Debug.LogError("Nav Mesh Agent not found!");
            }
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
                hit.transform.gameObject.TryGetComponent(out IDamageable target);
                
                if(!_fighter.CanAttack(target)) continue;
                
                if(Input.GetMouseButton(0) && target !=_damageable)
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
