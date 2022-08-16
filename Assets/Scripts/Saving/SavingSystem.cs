using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

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
                Transform playerTransform = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                SerializableVector3 position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position);
            }
        }

        public void Load(string savefile)
        {
            string path = GetPathFromSaveFile(savefile);
            using (FileStream stream = File.Open(path,FileMode.Open))
            {
                Transform playerTransform = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter();
                SerializableVector3 position = (SerializableVector3)formatter.Deserialize(stream);
                playerTransform.position = position.ToVector();
                print(playerTransform.position);
            }
        }

        private string GetPathFromSaveFile(string savefile)
        {
            return Path.Combine($"{Application.persistentDataPath}",$"{savefile}.sav");
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}