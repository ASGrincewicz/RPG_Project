using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] public int _currentLevel;
        [SerializeField][Range(1,99)] private int _startingLevel= 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression;
        [SerializeField] private GameObject _levelUpParticleEffect;
        private Experience _experience;
        public event Action onLevelUp;

        private void Start()
        {
            if (TryGetComponent(out _experience) && _characterClass == CharacterClass.Player)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
            _currentLevel = CalculateLevel();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > GetLevel())
            {
                _currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp?.Invoke();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat,_characterClass, CalculateLevel());
        }

        private int GetLevel()
        {
            if (_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
        }
        public int CalculateLevel()
        {
            if (!TryGetComponent(out _experience)) return _startingLevel;
            float currentXP = _experience.GetXP();

            int penultimateLevel = _progression.GetLevels(_characterClass, Stat.XPtoLevelUp);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(Stat.XPtoLevelUp, _characterClass, level);
                if (xpToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
