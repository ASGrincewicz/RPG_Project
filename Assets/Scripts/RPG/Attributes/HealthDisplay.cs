using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthValueText;
        private Health _health;

        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.TryGetComponent(out _health);
            }
        }

        private void Update()
        {
            _healthValueText.text = $"{_health.GetPercentage():0}%";
        }
    }
}
