using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1,99)] private int _startingLevel= 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression;
        private Experience _experience;
        
        public float GetStat(Stat stat)
        {
            return _progression.GetStat(stat,_characterClass, GetLevel());
        }

        public int GetLevel()
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
