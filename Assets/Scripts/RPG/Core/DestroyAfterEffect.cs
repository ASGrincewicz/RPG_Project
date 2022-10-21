using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private void Update()
        {
            if (!TryGetComponent(out ParticleSystem particle)) return;
            
            if (!particle.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
