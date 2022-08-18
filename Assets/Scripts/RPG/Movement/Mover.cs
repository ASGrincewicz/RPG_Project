using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private float _maxSpeed = 5.66f;
        private Health _health;
        private NavMeshAgent _navMeshAgent;
        private Transform _transform;
        private Animator _animator;
        private readonly int _forwardSpeedParameter = Animator.StringToHash("ForwardSpeed");

        private void Start()
        {
            _health = GetComponent<Health>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _transform = transform;
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.SetDestination(destination);
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            _navMeshAgent.isStopped = true;
        }

        public void Cancel()
        {
           print("Move cancelled.");
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
    }
}