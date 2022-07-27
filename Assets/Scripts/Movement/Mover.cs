using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Camera _mainCamera;
    private Transform _transform;
    private Animator _animator;
    private readonly int _forwardSpeedParameter = Animator.StringToHash("ForwardSpeed");

    private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _mainCamera = Camera.main;
            _transform = transform;
            _animator = GetComponent<Animator>();
        }
    
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
           MoveToCursor();
        }
        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        bool hasHit = Physics.Raycast(ray, out hitInfo);
        if (hasHit)
        {
            _navMeshAgent.SetDestination(hitInfo.point);
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
        _animator.SetFloat(_forwardSpeedParameter,speed);
    }
}
