using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class XPDisplay: MonoBehaviour
    {
        [SerializeField] private TMP_Text _xpValueText;
        private Experience _experience;
        private BaseStats _baseStats;

        private void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.TryGetComponent(out _experience);
                player.TryGetComponent(out _baseStats);
            }
            else
            {
                Debug.LogError("Player not found.");
            }
        }

        private void Update()
        {
            _xpValueText.text = $"{_experience.GetXP():N1}/{_baseStats.GetBaseStat(Stat.XPtoLevelUp)}";
        }
    }
}