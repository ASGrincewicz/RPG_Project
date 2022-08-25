using System.Collections;
using UnityEngine;
using Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper: MonoBehaviour
    {
        [SerializeField] private float _fadeInTime = 1.0f;
        private const string _defaultSaveFile = "RPG_Save";
        private SavingSystem _savingSystem;

        private void Awake()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();
        }

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            TryGetComponent(out SavingSystem savingSystem);
            yield return  savingSystem.LoadLastScene(_defaultSaveFile);
            yield return fader.FadeIn(_fadeInTime);
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
            _savingSystem.Save(_defaultSaveFile);
        }
        
        public void Load()
        {
            _savingSystem.Load(_defaultSaveFile);
        }
    }
}