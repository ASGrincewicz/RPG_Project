using UnityEngine;
namespace RPG.Utilities
{
    public static class ExtensionMethods
    {

        public static float GetDistanceTo(this Transform transform, Transform target)
        {
            var distance = Vector3.Distance(transform.position, target.position);
            return distance;
        }

        public static bool GetIsInRange(this Transform transform, Transform target, float range)
        {
            return Vector3.Distance(transform.position, target.position) <= range;
        }
        
        
    }
}