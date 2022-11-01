using System;
using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _healthComponent;
        [SerializeField] private RectTransform _foregroundBar;
        [SerializeField] private float _visibilityTime = 5.0f;

        private void Start()
        {
            UpdateHealthBar();
            if (_healthComponent.IsDead)
            {
                HandleCharacterDeath();
            }
        }

        public void UpdateHealthBar()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
            _foregroundBar.localScale = new Vector3(_healthComponent.GetFraction(), 1, 1);
            Invoke(nameof(DisableHealthBar),_visibilityTime);
        }
        /// <summary>
        /// Sets Health Bar to 0 and disables.
        /// </summary>
        public void HandleCharacterDeath()
        {
            _foregroundBar.localScale = new Vector3(0, 1, 1);
           Invoke(nameof(DisableHealthBar),0.5f);
        }

        private void DisableHealthBar()
        {
            gameObject.SetActive(false);
        }
    }
}