using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public LazyValue<int> _currentLevel;
        [SerializeField][Range(1,99)] private int _startingLevel= 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression;
        [SerializeField] private GameObject _levelUpParticleEffect;
        [SerializeField] private bool _shouldUseModifiers = false;
        private Experience _experience;
        public event Action onLevelUp;

        private void Awake()
        {
            TryGetComponent(out _experience);
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (_experience != null && _characterClass == CharacterClass.Player)
            {
                _experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null && _characterClass == CharacterClass.Player)
            {
                _experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
           _currentLevel.ForceInit();
           // _currentLevel = CalculateLevel();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > GetLevel())
            {
                _currentLevel.value = newLevel;
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
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (_currentLevel.value < 1)
            {
                _currentLevel.value = CalculateLevel();
            }
            return _currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        private int CalculateLevel()
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
