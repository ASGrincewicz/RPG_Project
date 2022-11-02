using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent _playSoundFX;
        public void OnHit()
        {
            _playSoundFX?.Invoke();
        }
    }
}