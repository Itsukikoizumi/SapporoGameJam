using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Surviver
{
    /// <summary>
    /// 弾たちを管理するクラスです
    /// </summary>
    public class EnemyBulletPool : PoolBase<EnemyBullet>
    {
        // poolBaseの方で弾をリストで持っています。

        /// <summary>
        /// 動かします
        /// </summary>
        public void OnUpdate()
        {
            // フレームごとに処理して弾を動かします
            foreach (var e in _list)
            {
                if (e.gameObject.activeInHierarchy)
                {
                    e.OnUpdate();
                }
            }
        }

        /// <summary>
        /// 弾を出現させる。
        /// </summary>
        public EnemyBullet SpawnBullet(Vector3 position, Vector3 direction, float speed, float offsetAngle = 0f, float scale = 1f)
        {
            var bullet = Create();
            bullet.Shoot(position, direction, speed, offsetAngle, scale);
            return bullet;
        }
    }
}