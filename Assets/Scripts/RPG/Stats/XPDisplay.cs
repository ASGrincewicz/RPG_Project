using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class XPDisplay: MonoBehaviour
    {
        [SerializeField] private TMP_Text _xpValueText;
        private Experience _experience;

        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.TryGetComponent(out _experience);
            }
            else
            {
                Debug.LogError("Player not found.");
            }
        }

        private void Update()
        {
            _xpValueText.text = $"{_experience.GetXP():N1}";
        }
    }
}