using System.Security.Cryptography;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _targetToDestroy;
        private void Update()
        {
            if (!TryGetComponent(out ParticleSystem particle)) return;
            
            if (!particle.IsAlive())
            {
                Destroy(_targetToDestroy != null ? _targetToDestroy : gameObject);
            }
        }
    }
}
