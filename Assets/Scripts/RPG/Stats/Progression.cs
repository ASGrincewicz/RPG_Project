using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression",menuName = "Stats/ Create New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass _characterClass;
            [SerializeField] private int[] _health;
        }
    }
}
