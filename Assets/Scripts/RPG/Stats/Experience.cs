using System;
using Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;
        

        public event Action onExperienceGained;

        public void GainXP(float xp)
        {
            _experiencePoints += xp;
            onExperienceGained?.Invoke();
        }

        public float GetXP()
        {
            return _experiencePoints;
        }
        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}