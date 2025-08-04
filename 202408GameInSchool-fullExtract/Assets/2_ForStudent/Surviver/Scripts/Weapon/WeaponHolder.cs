using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 武器たちを持つクラス
    /// </summary>
    public class WeaponHolder : MonoBehaviour
    {
        [SerializeField]
        private List<WeaponBase> _list = new List<WeaponBase>();

        private Queue<int> _lvUpQueue = new Queue<int>();
        private bool _lvUping = false;

        public void Initialize()
        {
            _list.ForEach(weapon => weapon.Initialize());
        }

        public void ResetStatus()
        {
            _list.ForEach(weapon => weapon.ResetStatus());
        }

        public void OnUpdate()
        {
            _list.ForEach(weapon => weapon.OnUpdate());

            // もしレベルアップが連続していれば...
            if (_lvUpQueue.Any() && !_lvUping)
            {
                var lvCount = _lvUpQueue.Dequeue();
                OpenLvUpPopupInternal(lvCount);
            }
        }

        /// <summary>
        /// すべての武器レベルを設定する
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(int level)
        {
            _list.ForEach(weapon => weapon.SetLevel(level));
        }

        public void SetLv1DefaultWeapon()
        {
            _list[0].SetLevel(1);
        }

        /// <summary>
        /// weaponholderからランダムで武器をチョイスして、レベルUP選択させる
        /// </summary>
        public void OpenLvUpPopup(int lvCount)
        {
            if(!_lvUping)
            {
                OpenLvUpPopupInternal(lvCount);
            }
            else
            {
                _lvUpQueue.Enqueue(lvCount);
            }
        }

        private void OpenLvUpPopupInternal(int lvCount)
        {
            _lvUping = true;
            // 4つの選択肢を、レベルアップの数だけ用意する。つまり  xxxx|xxxx|xxxx みたいな感じ
            var infoGroups = Enumerable.Repeat(0, lvCount).Select(_ => GetUpgradeInfo());

            var upgradePopup = PopupManager.Instance.Open<UpgradePopup>();
            upgradePopup.SetButtons(new Queue<List<UpgradePopup.ButtonInfo>>(infoGroups));
            upgradePopup.OnClose = () => _lvUping = false;
        }

        private List<UpgradePopup.ButtonInfo> GetUpgradeInfo()
        {
            // シャッフルして先頭から4つ取り出し、Hierarchy上の順番に並ばせる
            var weapons = _list.Shuffle().Take(4).OrderBy(w => w.transform.GetSiblingIndex());

            var infos = weapons.Select(weapon => 
            { 
                var lvUpDescription = weapon.Level == 0 ? "獲得" : $"Lv {weapon.Level}→{weapon.Level + 1}";

                // アプグレ画面上での1ボタン情報を作成
                var info = new UpgradePopup.ButtonInfo
                {
                    Description = $"{weapon.WeaponName}\n{lvUpDescription}",
                    OnClick = () =>
                    {
                        weapon.SetLevel(weapon.Level + 1);
                    }
                };
                return info;

            }).ToList();
            return infos;
        }

#if UNITY_EDITOR
        // オブジェクトへのアタッチタイミングで子のオブジェのWeaponBaseたちをリストに突っ込む
        private void Reset()
        {
            _list.Clear();
            foreach (Transform child in transform)
            {
                var weapon = child.GetComponent<WeaponBase>();
                if (weapon != null)
                {
                    _list.Add(weapon);
                }
            }
        }
#endif
    }
}
