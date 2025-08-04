using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Surviver
{
    /// <summary>
    /// 弾出すクラス
    /// </summary>
    public class FireBallThrower : WeaponBase
    {
        [SerializeField]
        private PlayerBulletPool _bulletPool;

        [SerializeField]
        private float _baseFirePerSeconds = 1f;
        [SerializeField]
        private float _baseBulletPerFire = 0.5f;

        [SerializeField]
        private float _angleOffset = 15f;

        private float _firePerSeconds = 1f;
        private int _bulletPerFire = 1;
        private float _fireInterval = 0f;

        private Vector3 _prePosi = Vector3.zero;

        public override void ResetStatus()
        {
            _bulletPool.ClearAll();
            _bulletPool.Prepare(30);
        }

        public override void OnUpdateInternal()
        {
            _fireInterval -= Time.deltaTime;
            if(_fireInterval <= 0)
            {
                _fireInterval = 1f / _firePerSeconds;
                
                // 一番近い敵を探す
                var targets = GameObject.FindGameObjectsWithTag("EnemyCharacter");
                Vector3 dir;

                if (targets.Length == 0)
                {
                    dir = transform.position - _prePosi;
                }
                else
                {
                    var closeTarget = targets
                        .Select(enemy => (enemy, Vector3.Distance(transform.position, enemy.transform.position)))
                        .OrderBy(pair => pair.Item2)
                        .FirstOrDefault().enemy;

                    dir = closeTarget.transform.position - transform.position;
                }

                for (int i = 0; i < _bulletPerFire; i++)
                {
                    // offsetAngleを15度ずつずらして弾を撃つ
                    var offsetAngle = _angleOffset * (i - (_bulletPerFire - 1) / 2);

                    var bullet = _bulletPool.SpawnBullet(transform.position, dir, offsetAngle);
                }
            }
            _prePosi = transform.position;

            // 弾たちを動かす
            _bulletPool.OnUpdate();
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);
            if (level <= 0)
                return;

            _firePerSeconds = _baseFirePerSeconds * (1f + 0.1f * (level - 1));
            _bulletPerFire = 1 + (int)((level * _baseBulletPerFire) - 1);
        }
    }
}
