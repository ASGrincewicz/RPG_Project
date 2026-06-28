using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.UI.HealText
{
    public class HPTextUI : MonoBehaviour
    {
        public TMP_Text Text;
        private IObjectPool<HPTextUI> _pool;
       
        //Animation Event
        private void Release()
        {
            _pool.Release(this);
        }

        public void SetPool(IObjectPool<HPTextUI> pool)
        {
            _pool = pool;
        }
    }
}