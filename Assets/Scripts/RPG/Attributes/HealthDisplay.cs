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
            _healthValueText.text = $"{_health.HealthPoints.value:00}/{_health.GetMaxHealth()}";
        }
    }
}
