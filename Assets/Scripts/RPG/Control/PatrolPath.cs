using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float _waypointGizmoRadius = .25f;
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(GetWaypoint(i),_waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }
        #endif

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

