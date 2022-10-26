using System;
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
            TryGetComponent(out _canvasGroup);
        }

        public void FadeOutImmediate()
        {
            _canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (TryGetComponent(out _canvasGroup) && _canvasGroup.alpha < 1) //alpha is not one
            {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (TryGetComponent(out _canvasGroup) && _canvasGroup.alpha > 0) //alpha is not zero
            {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
