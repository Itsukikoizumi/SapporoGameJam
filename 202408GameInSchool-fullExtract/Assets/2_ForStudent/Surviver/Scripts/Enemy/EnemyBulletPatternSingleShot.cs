using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Surviver
{
    public class EnemyBulletPatternSingleShot : EnemyBulletPattern
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
                _pool.SpawnBullet(transform.position, dir, _bulletSpeed, 0f, _bulletScale + (0.1f * _level));
            }
        }
    }
}
