using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Core
{
    public class RandomAudio : MonoBehaviour
    {
        
        [SerializeField] private AudioClip[] _audioClips;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponentInParent<AudioSource>();
        }

        public void PlayRandom()
        {
            AudioClip clip = _audioClips[Random.Range(0, _audioClips.Length - 1)];
            _audioSource.PlayOneShot(clip);
        }
    }
}