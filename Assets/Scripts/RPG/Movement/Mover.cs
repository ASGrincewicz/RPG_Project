using RPG.Attributes;
using RPG.Core;
using Saving;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float _maxSpeed = 5.66f;
        [SerializeField] private float _maxNavMeshPathLength = 10.0f;
        private Health _health;
        private ActionScheduler _actionScheduler;
        private NavMeshAgent _navMeshAgent;
        private Transform _transform;
        private Animator _animator;
        private readonly int _forwardSpeedParameter = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            TryGetComponent(out _health);
            TryGetComponent(out _actionScheduler);
            TryGetComponent(out _navMeshAgent);
            _transform = transform;
            TryGetComponent(out _animator);
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > _maxNavMeshPathLength) return false;

            return true;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.SetDestination(destination);
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            if (_navMeshAgent.isActiveAndEnabled)
            {
                _navMeshAgent.isStopped = true;
            }
        }

        public void Cancel()
        {
            if (_navMeshAgent.isActiveAndEnabled)
            {
                _navMeshAgent.isStopped = true;
            }
        }
        private void UpdateAnimator()
        {
            //get global velocity from navmeshagent
            Vector3 velocity = _navMeshAgent.velocity;
            //Convert to local value relative to the character
            Vector3 localVelocity = _transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            //Set animator blend value to desired forward speed.
            _animator.SetFloat(_forwardSpeedParameter, speed);
        }
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0.0f;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length -1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            //NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.enabled = false;
            _transform.position = position.ToVector();
            _navMeshAgent.Warp(position.ToVector());
            _navMeshAgent.enabled = true;
            _actionScheduler.CancelAction();
        }
    }
}