using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float _experiencePoints;

        public void GainXP(float xp)
        {
            _experiencePoints += xp;
        }
    }
}