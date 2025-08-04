using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Surviver
{
    /// <summary>
    /// 弾たちを管理するクラスです
    /// </summary>
    public class PlayerBulletPool : PoolBase<PlayerBullet>
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
        public PlayerBullet SpawnBullet(Vector3 position, Vector3 direction, float offsetAngle = 0f)
        {
            var bullet = Create();
            bullet.Shoot(position, direction, offsetAngle);
            return bullet;
        }
    }
}