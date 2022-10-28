using System;
using Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour,ISaveable
    {
        [SerializeField]  private bool _hasPlayed = false;
        private PlayableDirector _director;

        private void OnEnable()
        {
            TryGetComponent(out PlayableDirector _director);
            _director.played += SetPlayed;
        }

        private void OnDisable()
        {
            TryGetComponent(out PlayableDirector _director);
            _director.played -= SetPlayed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed) return;
            if (!other.CompareTag("Player")) return;
            
            if (TryGetComponent(out _director))
            {
                _director.Play();
            }
        }

        private void SetPlayed(PlayableDirector director)
        {
            _hasPlayed = true;
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
