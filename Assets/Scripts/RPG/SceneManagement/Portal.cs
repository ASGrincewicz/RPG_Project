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

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
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
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            Portal exitPortal = GetExitPortal();
            UpdatePlayer(exitPortal);
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

