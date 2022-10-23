using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Saving
{
    public class SavingSystem: MonoBehaviour
    {
        public bool CheckIfSaveFileExists(string saveFile)
        {
            if (File.Exists(GetPathFromSaveFile(saveFile)))
            {
                return true;
            }

            return false;
        }
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }
        
        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void Delete(string saveFile)
        {
            if (File.Exists(saveFile))
            {
                File.Delete(GetPathFromSaveFile(saveFile));
            }
        }
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = (int)state["lastSceneBuildIndex"];
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            RestoreState(state);
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine($"{Application.persistentDataPath}",$"{saveFile}.sav");
        }

        private void SaveFile(string saveFile, Object state)
        {
            string path = GetPathFromSaveFile(saveFile);
          
           using (FileStream stream = File.Open(path, FileMode.Create))
           {
               
               BinaryFormatter formatter = new BinaryFormatter();
               formatter.Serialize(stream, state);
           }
        }

       
        private Dictionary<string, object> LoadFile(string saveFile)
        { 
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
           using (FileStream stream = File.Open(path,FileMode.Open))
           {
               BinaryFormatter formatter = new BinaryFormatter();
               return (Dictionary<string, object>)formatter.Deserialize(stream);
           }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
               state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }
    }
}