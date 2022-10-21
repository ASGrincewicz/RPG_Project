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
        private Fader _fader;
        private void Awake()
        {
            _savingSystem = FindObjectOfType<SavingSystem>();
        }

        private IEnumerator Start()
        {
            _fader = FindObjectOfType<Fader>();
            TryGetComponent(out SavingSystem savingSystem);
            if (_savingSystem.CheckIfSaveFileExists(_defaultSaveFile))
            {
                _fader.FadeOutImmediate();
                print("Save File Found: Fading out");
                yield return  savingSystem.LoadLastScene(_defaultSaveFile);
            }
            else
            {
                yield return _fader.FadeIn(_fadeInTime);
                print("No Save File Found.");
            }
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