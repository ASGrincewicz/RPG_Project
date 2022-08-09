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
        private void Start()
        {
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<PlayableDirector>().played += DisableControl;
            _player = GameObject.FindWithTag("Player");
        }

        private void EnableControl(PlayableDirector director)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }

        private void DisableControl(PlayableDirector director)
        { 
            _player.GetComponent<ActionScheduler>().CancelAction();
            _player.GetComponent<PlayerController>().enabled =  false;
        }
    }
}