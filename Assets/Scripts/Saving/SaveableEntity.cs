using System;
using UnityEditor;
using UnityEngine;

namespace Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string _uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return "";
        }

        public object CaptureState()
        {
            print($"Capturing State for {GetUniqueIdentifier()}.");
            return null;
        }

        public void RestoreState(object state)
        {
            print($"Restoring State for {GetUniqueIdentifier()}.");
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_uniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            print($"Editing {this.name}");
        }
#endif
    }
}