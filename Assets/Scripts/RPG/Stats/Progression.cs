using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression",menuName = "Stats/ Create New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        public int GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass c in _characterClasses)
            {
                if (c.CharacterClass == characterClass)
                {
                    return c.Health[level - 1];
                }
            }

            return 0;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [field: SerializeField] public CharacterClass CharacterClass;
            [field: SerializeField] public int[] Health;
        }
    }
}
