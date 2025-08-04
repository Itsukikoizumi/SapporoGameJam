using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// サバイバーゲーにおけるプレイヤーの弾。
    /// 敵に接触したときに攻撃を行う。
    /// </summary>
    public class PlayerBullet : MonoBehaviour
    {
        [SerializeField]
        private int _attackPower = 10;

        [SerializeField]
        private float _knockbackPower = 1f;

        [SerializeField]
        private float _speed = 2f;

        [SerializeField]
        private float _lifeTime = 6f;
        private float _currentLifeTime = 0f;

        [Header("ヒット時に消えるか。falseの場合は消えないので貫通弾みたいになります")]
        [SerializeField]
        private bool _removeByHit = true;

        public virtual void Shoot(Vector3 position, Vector3 direction, float offsetAngle = 0f)
        {
            transform.position = position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90; // 上向きがデフォなので
            transform.rotation = Quaternion.Euler(0, 0, angle + offsetAngle);
            gameObject.SetActive(true);

            _currentLifeTime = _lifeTime;
        }

        public void OnUpdate()
        {
            if(_currentLifeTime > 0f)
            {
                // 移動処理
                transform.position += transform.up * _speed * Time.deltaTime;

                // 寿命処理
                _currentLifeTime -= Time.deltaTime;
                if (_currentLifeTime <= 0)
                {
                    LifeEnd();
                }
            }
        }

        public virtual void LifeEnd()
        {
            _currentLifeTime = 0;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "EnemyCharacter")
            {
                var enemy = collision.GetComponent<EnemyCharacter>();

                if(_removeByHit)
                {
                    enemy.OnAttacked(_attackPower, _knockbackPower, transform.position);
                    LifeEnd();
                }
                else
                {
                    // 貫通弾の場合...
                    var uniqueId = GetInstanceID(); // このオブジェクトの一意なIDを取得
                    // 連続ヒットを防ぐため、ヒット後0.8秒は当たらないよう登録いただく
                    var damageInfo = new EnemyCharacter.DamageInfo(){AttackId = uniqueId, WaitTime = 0.8f};
                    enemy.OnAttacked(_attackPower, _knockbackPower, transform.position, damageInfo);
                }
            }
        }

    }
}
