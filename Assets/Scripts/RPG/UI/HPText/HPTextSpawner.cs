using UnityEngine;
using UnityEngine.Pool;

namespace RPG.UI.HealText
{
    public class HPTextSpawner : MonoBehaviour
    {
        [SerializeField] private HPTextUI textUIPrefab;
        [SerializeField] private int _maxPoolSize;
        private IObjectPool<HPTextUI> _textPool;
        private float _tempAmount = 0;

        private void Awake()
        {
            _textPool = new ObjectPool<HPTextUI>(CreateText,OnGet,OnRelease,OnTextDestroy,maxSize: _maxPoolSize);
        }
        
        private HPTextUI CreateText()
        {
            HPTextUI textUI = Instantiate(textUIPrefab,transform);
            textUI.SetPool(_textPool);

            return textUI;
        }

        private void OnGet(HPTextUI hpTextUI)
        {
            hpTextUI.Text.text = $"{_tempAmount}";
            hpTextUI.gameObject.SetActive(true);
        }

        private void OnRelease(HPTextUI hpTextUI)
        {
            if (hpTextUI.isActiveAndEnabled)
            {
                hpTextUI.gameObject.SetActive(false);
            }
        }

        private void OnTextDestroy(HPTextUI hpTextUI)
        {
            Destroy(hpTextUI.gameObject);
        }
        
        public void Spawn(float value)
        {
            _tempAmount = value;
            _textPool.Get();
        }
    }
}