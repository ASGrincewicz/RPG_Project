using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 3.0f;
        [SerializeField] private float _suspicionWaitTime = 3.0f;

        [Header("Patrol Configuration")] 
        [SerializeField, Range(0,1)] private float _patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath _patrolPath = null;
        [SerializeField] private float _waypointTolerance = 1.0f;
        [SerializeField] private float _waypointDwellTime = 2.0f;
        

        private Vector3 _guardPosition;
        private int _currentWaypointIndex = 0;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private float _timeAtWaypoint = Mathf.Infinity;
        //Cached References
        private Fighter _fighter;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Transform _transform;
        private GameObject _player;
        private IDamageable _target;
        private Health _health;

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

            if (!TryGetComponent(out _actionScheduler))
            {
                Debug.LogError("Action Scheduler not found.");
            }

            if (!TryGetComponent(out _health))
            {
                Debug.LogError("Health not found!");
            }
            _transform = transform;
            _guardPosition = _transform.position;
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            if (_player == null)
            {
                print("Have Not Found Player!");
            }
        }

        private void Update()
        {
            if(_health.IsDead) return;
            if (IsInRange(_player.gameObject))
            {
                if (_fighter.CanAttack(_target))
                {
                    AttackBehavior();
                }
            }
            else if(_timeSinceLastSawPlayer < _suspicionWaitTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition;
            if (_patrolPath != null)
            {
               //Change speed to Patrol Speed.
                if (AtWaypoint())
                {
                    _timeAtWaypoint = 0;
                    
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
               
                
                if (_timeAtWaypoint > _waypointDwellTime)
                {
                    _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
                }
            }
            else
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
           
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(_transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
           _currentWaypointIndex =  _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void SuspicionBehavior()
        {
            _actionScheduler.CancelAction();
        }

        private void AttackBehavior()
        {
            _timeSinceLastSawPlayer = 0;
            //Change Speed to Chase Speed.
            _fighter.Attack(_target);
        }

        private bool IsInRange(GameObject target)
        {
            float distance = Vector3.Distance(_transform.position, target.transform.position);
            if (_target == null)
            { 
                target.TryGetComponent(out _target);
            }
            
            if (distance < _chaseDistance && _target != null)
            {
                return true;
            }

            return false;
        }
        //Called by Unity
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
        #endif
    }
}