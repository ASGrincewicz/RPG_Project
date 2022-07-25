using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _navMeshAgent;
    
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        if (_navMeshAgent.destination == _target.position) return;
        _navMeshAgent.SetDestination(_target.position);
        //Debug.Log("Agent Set");
    }
}
