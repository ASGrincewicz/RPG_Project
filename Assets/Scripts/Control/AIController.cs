using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float _chaseDistance = 3.0f;

        private Vector3 _guardPosition;
        //Cached References
        private Fighter _fighter;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Transform _transform;
        private GameObject _player;
        private IDamageable _target;
        private Health _health;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _health = GetComponent<Health>();
            _transform = transform;
            _guardPosition = _transform.position;
        }

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            if (_player == null)
            {
                print("Have Not Found Player!");
            }
        }

        private void Update()
        {
            if(_health.IsDead) return;
            if (IsInRange(_player.gameObject))
            {
                if (_fighter.CanAttack(_target))
                {
                    _fighter.Attack(_target);
                }
            }
            else
            {
                _mover.StartMoveAction(_guardPosition);
            }
        }

        private bool IsInRange(GameObject target)
        {
            float distance = Vector3.Distance(_transform.position, target.transform.position);
            if (_target == null)
            {
                _target = target.GetComponent<IDamageable>();
            }
            
            if (distance < _chaseDistance && _target != null)
            {
                return true;
            }

            return false;
        }
        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}