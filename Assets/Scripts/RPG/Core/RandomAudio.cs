using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Core
{
    public class RandomAudio : MonoBehaviour
    {
        
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField, Range(0,3)] private float _minimumPitch;
        [SerializeField, Range(0,3)] private float _maxPitch;
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

        public void PitchRandom()
        {
            _audioSource.pitch = Random.Range(_minimumPitch, _maxPitch);
        }
    }
}