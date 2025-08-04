using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 武器の基底クラス
    /// </summary>
    public class WeaponBase : MonoBehaviour
    {
        public string WeaponName = "武器の名前";
        public int Level { get; private set; } = 1;

        public virtual void Initialize()
        {

        }

        public virtual void ResetStatus()
        {

        }

        public void OnUpdate()
        {
            if(Level > 0)
            {
                OnUpdateInternal();
            }
        }

        public virtual void OnUpdateInternal()
        {

        }

        public virtual void SetLevel(int level)
        {
            Level = level;

            if(Level <= 0)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
