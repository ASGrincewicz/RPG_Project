using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField][Range(1,99)] private int _startingLevel= 1;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression;

        public float GetHealth()
        {
            return _progression.GetHealth(_characterClass, _startingLevel);
        }
    }
}
