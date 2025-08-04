using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Surviver
{
    public class EnemyBulletPattern3WayShot : EnemyBulletPattern
    {
        [SerializeField]
        private float _bulletPerSecond = 0.2f;

        [SerializeField]
        private float _bulletSpeed = 5.0f;

        [SerializeField]
        private float _bulletScale = 1.0f;

        private float _currentInterval = 0f;


        public override void ResetStatus(int level)
        {
            base.ResetStatus(level);
            _currentInterval = 1f / _bulletPerSecond;
        }

        public override void OnUpdate()
        {
            _currentInterval -= Time.deltaTime;
            if (_currentInterval <= 0f)
            {
                var dir = _playerTransform.position - transform.position; // プレイヤーに向かうベクトルを取得

                // 3方向に弾を発射
                for(var i = 0; i < 3; i++)
                {
                    var angle = 30 * (i - 1); // -30, 0, 30
                    _pool.SpawnBullet(transform.position, dir, _bulletSpeed, angle, _bulletScale + (0.1f * _level));
                }
                _currentInterval = 1f / _bulletPerSecond;
            }
        }
    }
}
