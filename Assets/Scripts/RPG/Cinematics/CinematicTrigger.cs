using Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour,ISaveable
    {
        [SerializeField]  private bool _hasPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed) return;
            if (!other.CompareTag("Player")) return;
            
            if (TryGetComponent(out PlayableDirector director))
            {
                director.Play();
            }
            _hasPlayed = true;
            print($"Cinematic: Has Played ={_hasPlayed}.");
        }

        public object CaptureState()
        {
            return _hasPlayed;
        }

        public void RestoreState(object state)
        {
            _hasPlayed = (bool)state;
        }
    }
}
