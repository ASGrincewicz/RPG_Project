using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Camera _mainCamera;

    private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _mainCamera = Camera.main;
        }
    
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
           MoveToCursor();
        }
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
}
