﻿using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _enemyHealthValue;

        private Fighter _playerFighter;

        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (!player.TryGetComponent(out _playerFighter))
            {
                Debug.LogError("Fighter scripts not found on Player!");
            }
        }

        private void Update()
        {
            if (_playerFighter == null)
            {
                _enemyHealthValue.text = $"N/A";
            }

            IDamageable target = _playerFighter.GetTarget();
            _enemyHealthValue.text = target == null ? $"No Target" : $"{target.HealthPoints.value:0}/{target.GetMaxHealth():0}";
        }
    }
}