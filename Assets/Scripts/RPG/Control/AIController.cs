using System;
using GameDevTV.Utils;
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
        [SerializeField] private float _aggroCooldownTime = 5.0f;
        [SerializeField] private LayerMask _blockingLayer;
        [SerializeField] private float _shoutDistance = 10.0f;

        [Header("Patrol Configuration")] 
        [SerializeField, Range(0,1)] private float _patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath _patrolPath = null;
        [SerializeField] private float _waypointTolerance = 1.0f;
        [SerializeField] private float _waypointDwellTime = 2.0f;
        

        private LazyValue<Vector3> _guardPosition;
        private int _currentWaypointIndex = 0;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeAtWaypoint = Mathf.Infinity;
        private float _timeSinceAggravated = Mathf.Infinity;
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

            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void OnEnable()
        {
            _health.OnTakeDamage += Aggravate;
        }

        private void OnDisable()
        {
            _health.OnTakeDamage -= Aggravate;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            _transform = transform;
            _guardPosition.ForceInit();
            _player = GameObject.FindWithTag("Player");
            if (_player == null)
            {
                print("Have Not Found Player!");
            }
        }

        private void Update()
        {
            if(_health.IsDead) return;
            if (IsAggravated()&& _fighter.CanAttack(_target))
            {
                AttackBehavior();
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

        public void Aggravate(GameObject target)
        {
            target.TryGetComponent(out _target);
            if (target != null)
            {
                print($"{gameObject.name} is targeting {_target.GetTransform().gameObject.name}");
            }
            _timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeAtWaypoint += Time.deltaTime;
            _timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = _guardPosition.value;
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
            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
          RaycastHit[] hits =  Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0);
          
          //Loop over all the hits
          foreach (RaycastHit hit in hits)
          {
              AIController ai = hit.collider.GetComponent<AIController>();
              if (ai == null) continue;
              ai.Aggravate(_target.GetTransform().gameObject);
          }
          //Find enemy components
          //aggravate those enemies
        }

        private bool IsAggravated()
        {
            _player.TryGetComponent(out _target);
            float distance = Vector3.Distance(_target.GetPosition(), transform.position);

            return distance < _chaseDistance || _timeSinceAggravated < _aggroCooldownTime;
        }
        //Called by Unity
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,_shoutDistance);
        }
        #endif
    }
}