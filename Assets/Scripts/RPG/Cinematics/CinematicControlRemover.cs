using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            if (TryGetComponent(out PlayableDirector director))
            {
                director.stopped += EnableControl;
                director.played += DisableControl;
            }
        }

        private void OnDisable()
        {
            if (TryGetComponent(out PlayableDirector director))
            {
                director.stopped -= EnableControl;
                director.played -= DisableControl;
            }
        }
        
        private void EnableControl(PlayableDirector director)
        {
            if (_player.TryGetComponent(out PlayerController playerController))
            {
                playerController.enabled = true;
            }
        }

        private void DisableControl(PlayableDirector director)
        { 
            if (_player.TryGetComponent(out PlayerController playerController))
            {
                playerController.enabled = false;
            }
            if (_player.TryGetComponent(out ActionScheduler actionScheduler))
            {
               actionScheduler.CancelAction();
            }
        }
    }
}