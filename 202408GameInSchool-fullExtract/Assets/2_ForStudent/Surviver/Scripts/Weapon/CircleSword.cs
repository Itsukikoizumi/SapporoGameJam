using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 回転する剣のクラス。
    /// </summary>
    public class CircleSword : WeaponBase
    {
        [SerializeField]
        private Spin _spin;

        [SerializeField]
        private Sword _swordOrigin;

        private List<Sword> _swordList = new List<Sword>();

        public override void Initialize()
        {
            _swordList.Add(_swordOrigin);
        }

        public override void ResetStatus()
        {
            _swordList.ForEach(sword => sword.gameObject.SetActive(false));
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);

            if (level <= 0)
                return;

            _spin.rotationSpeed = 180.0f + 20.0f * (level - 1);

            var swordCount = 1 + (int)(level - 1);

            (swordCount - _swordList.Count).Repeat(CreateSword);

            _swordList.ForEachWithIndex((index, sword) =>
            {
                if(index < swordCount)
                {
                    sword.gameObject.SetActive(true);
                    sword.transform.localRotation = Quaternion.Euler(0, 0, 360.0f / swordCount * index);
                }
                else
                {
                    sword.gameObject.SetActive(false);
                }
            });
        }

        private void CreateSword()
        {
            var sword = Instantiate(_swordOrigin, transform);
            _swordList.Add(sword);
        }
    }
}
