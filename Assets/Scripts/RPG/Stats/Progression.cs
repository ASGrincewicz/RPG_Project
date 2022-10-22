using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression",menuName = "Stats/ Create New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, int[]>> _lookupTable;

        public int GetStat(Stat stat, CharacterClass characterClass, int level)
        {
           BuildLookup();

           int[] levels = _lookupTable[characterClass][stat];

           if (levels.Length < level)
           {
               return 0;
           }

           return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();
            foreach (ProgressionCharacterClass progressionClass in _characterClasses)
            {
                Dictionary<Stat,int[]> statLookupTable = new Dictionary<Stat, int[]>();

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
            [field: SerializeField] public int[] Levels;
        }
    }
}
