using UnityEngine;

namespace RPG.Combat
{
    public interface ITargetable
    {
        public Vector3 GetPosition();
        public Transform GetTransform();
    }
}