using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed;

        private void Start()
        {
            _target = GameObject.FindWithTag("Player").transform;
            
        }

        private void Update()
        {
            if (_target != null)
            {
                transform.LookAt(GetAimLocation());
                transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
            }
        }

        private Vector3 GetAimLocation()
        {
            if (_target.TryGetComponent(out CapsuleCollider targetCapsule))
            {
                return _target.position + Vector3.up * targetCapsule.height / 1.5f;
            }
            else
            {
                return _target.position;
            }
        }
    }
}
