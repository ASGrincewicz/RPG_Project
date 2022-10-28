using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover))]
    public partial class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Fighter _fighter;
        private Camera _mainCamera;
        private Health _health;
        private IDamageable _damageable;

        [SerializeField] private CursorMapping[] _cursorMappings;
        [SerializeField] private float _maxNavMeshProjectionDistance = 1.0f;

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
            if (_mover.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.enabled = true;
            }
        }

        private void OnDisable()
        {
            if (_mover.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.enabled = false;
            }
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (_health.IsDead) return;
           // if(InteractWithCombat()) return;
           if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }
        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private RaycastHit[] RayCastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target,1);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            //Raycast to terrain
            //Find nearest navmesh point
            NavMeshHit navMeshHit;
            
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, _maxNavMeshProjectionDistance,
                NavMesh.AllAreas);
            //return true if found
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;
            return true;
        }
        
        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in _cursorMappings)
            {
                if (mapping.cursorType == type)
                {
                    return mapping;
                }
            }
            return _cursorMappings[0];
        }
    }
}
