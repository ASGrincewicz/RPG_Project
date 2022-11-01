using UnityEngine;
using UnityEngine.Pool;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _textPrefab;
        [SerializeField] private int _maxPoolSize;
        private IObjectPool<DamageText> _textPool;
        private float _tempAmount = 0;

        private void Awake()
        {
            _textPool = new ObjectPool<DamageText>(CreateText,OnGet,OnRelease,OnTextDestroy,maxSize: _maxPoolSize);
        }
        
        private DamageText CreateText()
        {
            DamageText text = Instantiate(_textPrefab,transform);
            text.SetPool(_textPool);

            return text;
        }

        private void OnGet(DamageText damageText)
        {
            damageText.Text.text = $"{_tempAmount}";
            damageText.gameObject.SetActive(true);
        }

        private void OnRelease(DamageText damageText)
        {
            if (damageText.isActiveAndEnabled)
            {
                damageText.gameObject.SetActive(false);
            }
        }

        private void OnTextDestroy(DamageText damageText)
        {
            Destroy(damageText.gameObject);
        }
        
        public void Spawn(float value)
        {
            _tempAmount = value;
            _textPool.Get();
        }
    }
}