using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Transform _transform;
        private Camera _mainCamera;
        private void Start()
        {
            _transform = transform;
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            _transform.position = _target.position;
        }
    }
}