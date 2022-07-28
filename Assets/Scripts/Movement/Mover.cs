using System;
using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Transform _transform;
        private Animator _animator;
        private Fighter _fighter;
        private readonly int _forwardSpeedParameter = Animator.StringToHash("ForwardSpeed");

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _transform = transform;
            _animator = GetComponent<Animator>();
            _fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            _fighter.Cancel();
            MoveTo(destination);
        }
        public void MoveTo(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
            _navMeshAgent.isStopped = false;
        }

        public void Stop()
        {
            _navMeshAgent.isStopped = true;
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