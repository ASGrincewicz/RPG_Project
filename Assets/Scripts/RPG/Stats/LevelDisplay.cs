using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelValueText;
        private BaseStats _baseStats;
        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.TryGetComponent(out _baseStats);
            }
        }

        private void Update()
        {
            _levelValueText.text = $"{_baseStats.CalculateLevel()}";
        }
    }
}