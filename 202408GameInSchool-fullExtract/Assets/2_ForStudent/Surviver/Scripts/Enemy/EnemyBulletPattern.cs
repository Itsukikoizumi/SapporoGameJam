using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    public class EnemyBulletPattern : MonoBehaviour
    {
        protected Transform _playerTransform;
        protected EnemyBulletPool _pool;
        protected int _level;

        public void Initialize(Transform playerTransform, EnemyBulletPool pool)
        {
            _playerTransform = playerTransform;
            _pool = pool;
        }

        public virtual void ResetStatus(int level)
        {
            level = _level;
        }

        public virtual void OnUpdate()
        {

        }
    }
}
