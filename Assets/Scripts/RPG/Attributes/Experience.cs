using Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;

        public void GainXP(float xp)
        {
            _experiencePoints += xp;
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