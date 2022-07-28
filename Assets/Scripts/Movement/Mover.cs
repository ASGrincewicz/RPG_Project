using System;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;
    private Animator _animator;
    private readonly int _forwardSpeedParameter = Animator.StringToHash("ForwardSpeed");

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = transform;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    public void MoveTo(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
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