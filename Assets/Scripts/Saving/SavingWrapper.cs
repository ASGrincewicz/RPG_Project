using System;
using UnityEngine;

namespace Saving
{
    public class SavingWrapper: MonoBehaviour
    {
        private const string _defaultSaveFile = "RPG_Save";

        private void Start()
        {
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
               Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(_defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(_defaultSaveFile);
        }
    }
}