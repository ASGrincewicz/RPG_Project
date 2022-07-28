using UnityEngine;
[RequireComponent(typeof(Mover))]
public class PlayerController : MonoBehaviour
{
    private Mover _mover;
    private Camera _mainCamera;
    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
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
           _mover.MoveTo(hitInfo.point);
        }
    }
}
