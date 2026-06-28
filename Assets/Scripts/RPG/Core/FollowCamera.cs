using UnityEngine;
using UnityEngine.Rendering;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _followDistance;
        //[SerializeField] private float _obstructionTolerance;
        //[SerializeField] private float _targetTolerance;
        private Transform _transform;
        [SerializeField] private Transform _obstruction;
        //private float _zoomSpeed = 2.0f;
        private Camera _mainCamera;
        private void Start()
        {
            _obstruction = _target;
            //_transform = transform;
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            transform.position = _target.position - _followDistance;
            ViewObstructed();
        }

        private void ViewObstructed()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _target.position - transform.position, out hit, 20.0f))
            {
                if (!hit.collider.gameObject.CompareTag("Player"))
                {
                    _obstruction = hit.transform;
                    //print($"{hit.transform.gameObject.name}");
                    MeshRenderer renderer = _obstruction.gameObject.GetComponentInChildren<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    }

                    /*float ObstructionToTransformDistance = Vector3.Distance(_obstruction.position, _transform.position);
                    float TransformToTargetDistance = Vector3.Distance(_transform.position, _target.position);
                    if (ObstructionToTransformDistance >= _obstructionTolerance && TransformToTargetDistance >= _targetTolerance)
                    {
                        _transform.Translate(Vector3.forward * (_zoomSpeed * Time.deltaTime));
                    }*/
                }
                else if(hit.collider.gameObject.CompareTag("Player"))
                {
                    if (_obstruction != null)
                    {
                        MeshRenderer renderer = _obstruction.gameObject.GetComponentInChildren<MeshRenderer>();
                        if (renderer != null)
                        {
                            renderer.shadowCastingMode = ShadowCastingMode.On;
                        }
                    }
                    _obstruction = null;
                    /*float TransformToTargetDistance = Vector3.Distance(_transform.position, _target.position);
                    if (TransformToTargetDistance < 4.5f)
                    {
                        _transform.Translate(Vector3.back *(_zoomSpeed * Time.deltaTime));
                    }*/
                }
            }
        }
    }
}