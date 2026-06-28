using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private Camera _camera;
        private Transform _transform;
        private void Start()
        {
           _camera = Camera.main;
           _transform = transform;
        }

        private void LateUpdate()
        {
            _transform.forward = _camera.transform.forward;
        }
    }
}