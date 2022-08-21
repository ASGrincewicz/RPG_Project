using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _hasPlayed = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_hasPlayed) return;
            GetComponent<PlayableDirector>().Play();
            _hasPlayed = true;
        }
    }
}
