using UnityEngine;
using RPG.Utilities;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 5.0f;
        private GameObject _player;
        private string _playerTag = "Player";
        private Transform _transform;
        private void Start()
        {
            _transform = transform;
            _player = GameObject.FindWithTag(_playerTag);
        }

        private void Update()
        {
            if (_player != null)
            {
                if(_transform.GetIsInRange(_player.transform, _chaseDistance))
                {
                    print($"{name} should give chase.");
                }
            }
        }
    }
}

