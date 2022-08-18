using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace Saving
{
    public class SavingSystem: MonoBehaviour
    {
        public void Save(string savefile)
        {
            string path = GetPathFromSaveFile(savefile);
            Debug.Log($"Saving to {path}");
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string savefile)
        {
            string path = GetPathFromSaveFile(savefile);
            using (FileStream stream = File.Open(path,FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
                
            }
        }

        private string GetPathFromSaveFile(string savefile)
        {
            return Path.Combine($"{Application.persistentDataPath}",$"{savefile}.sav");
        }

        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
               state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            return state;
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(stateDictionary[saveable.GetUniqueIdentifier()]);
            }
        }
    }
}