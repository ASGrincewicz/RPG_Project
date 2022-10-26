using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression",menuName = "Stats/ Create New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable;

        public float GetStat(Stat stat, CharacterClass characterClass, float level)
        {
           BuildLookup();

           float[] levels = _lookupTable[characterClass][stat];

           if (levels.Length < level)
           {
               return levels[levels.Length -1];
           }

           return levels[(int)(level - 1)];
        }

        public int GetLevels(CharacterClass characterClass, Stat stat)
        {
            BuildLookup();
            
            float[] levels = _lookupTable[characterClass][stat];

            return levels.Length;
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in _characterClasses)
            {
                Dictionary<Stat,float[]> statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.Stats)
                {
                    statLookupTable[progressionStat.Stat] = progressionStat.Levels;
                }
                
                _lookupTable[progressionClass.CharacterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [field: SerializeField] public CharacterClass CharacterClass;
            [field: SerializeField] public ProgressionStat[] Stats;
        }

        [System.Serializable]
        public class ProgressionStat
        {
            [field: SerializeField] public Stat Stat;
            [field: SerializeField] public float[] Levels;
        }
    }
}
