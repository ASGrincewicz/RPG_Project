﻿using System;
using System.Collections.Generic;
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
            return _uniqueIdentifier;
        }

        private static Dictionary<string, SaveableEntity> _globalLookup = new Dictionary<string, SaveableEntity>();

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                print($"{saveable}");
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_uniqueIdentifier");
            if (string.IsNullOrEmpty(property.stringValue)|| !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            _globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!_globalLookup.ContainsKey(candidate))
            {
                return true;
            }
           
            if (_globalLookup[candidate] == this)
            {
                return true;
            }

            if (_globalLookup[candidate] == null)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            if (_globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                _globalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}