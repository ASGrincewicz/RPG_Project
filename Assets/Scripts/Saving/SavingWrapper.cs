using System;
using UnityEngine;

namespace Saving
{
    public class SavingWrapper: MonoBehaviour
    {
        private const string _defaultSaveFile = "RPG_Save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                GetComponent<SavingSystem>().Save(_defaultSaveFile);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GetComponent<SavingSystem>().Load(_defaultSaveFile);
            }
        }
    }
}