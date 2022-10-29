using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public TMP_Text Text;
        private IObjectPool<DamageText> _pool;
       
        //Animation Event
        private void Release()
        {
            _pool.Release(this);
        }

        public void SetPool(IObjectPool<DamageText> pool)
        {
            _pool = pool;
        }
    }
}
