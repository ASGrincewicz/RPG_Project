using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationIdentifier
        {
            A,B,C,D,E,F,G,H
        }
        [SerializeField] private int _sceneToLoad;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;
        [Header("Transition Fade Configuration")]
        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _fadeInTime = 2f;
        [SerializeField] private float _fadeWaitTime = 1f;
        private WaitForSeconds _fadeDelay;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _fadeDelay = new WaitForSeconds(_fadeWaitTime);
            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeOutTime);
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            Portal exitPortal = GetExitPortal();
            UpdatePlayer(exitPortal);
            yield return _fadeDelay;
            yield return fader.FadeIn(_fadeInTime);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal portal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(portal._spawnPoint.position);
            player.transform.rotation = portal._spawnPoint.rotation;
        }

        private Portal GetExitPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if(portal == this) continue;
                if (portal._destination == _destination)
                {
                    return portal;
                }
            }
            return null;
        }
    }
}
