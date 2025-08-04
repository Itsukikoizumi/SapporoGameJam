using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SummonerWar
{
    /// <summary>
    /// 敵に当たるとダメージを与える弾。
    /// </summary>
    public class PlayerBullet : MonoBehaviour
    {
        public int attackPower = 10;
        public float lifeTime = 7f;

        private void Start()
        {
            // 一定時間で消える
            Destroy(gameObject, lifeTime);
        }

        /// <summary>
        /// ほかの物体にぶつかったとき
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 衝突したやつが敵じゃなかったら何もしない
            if (collision.collider.tag != "Enemy")
            {
                return;
            }

            // 敵にダメージを与える
            EnemyUnit enemyUnit = collision.collider.GetComponent<EnemyUnit>();
            if (enemyUnit != null)
            {
                var isDead = false;
                enemyUnit.GetDamage(attackPower, ref isDead, isPenetrateInvincible: true);
            }

            // 自分を破壊する
            Destroy(gameObject);
            // todo: 当たる敵が一体なのはなんか弱いので、2~3回当たるまで消えないようにするなど
        }
    }
}