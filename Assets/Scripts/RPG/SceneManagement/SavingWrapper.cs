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
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            _fader = FindObjectOfType<Fader>();
            TryGetComponent(out SavingSystem savingSystem);
            _fader.FadeOutImmediate();
            yield return savingSystem.LoadLastScene(_defaultSaveFile);
            yield return _fader.FadeIn(_fadeInTime);
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

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public void Delete()
        {
            _savingSystem.Delete(_defaultSaveFile);
        }
    }
}