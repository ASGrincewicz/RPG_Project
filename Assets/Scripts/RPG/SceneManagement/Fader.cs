using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private bool _isFadeOut = false;
        [SerializeField] private float _fadeTime;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            try
            {
                TryGetComponent(out _canvasGroup);
            }
            catch 
            {
                Debug.LogError("Canvas group not found.");
            }
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (_canvasGroup.alpha < 1) //alpha is not one
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (_canvasGroup.alpha > 0) //alpha is not zero
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
