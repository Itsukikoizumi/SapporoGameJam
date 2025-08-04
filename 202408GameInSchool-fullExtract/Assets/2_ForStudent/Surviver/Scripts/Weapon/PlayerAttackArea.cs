using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// サバイバーゲーにおけるプレイヤーの攻撃範囲
    /// 敵に接触したときに攻撃を行う。
    /// </summary>
    public class PlayerAttackArea : MonoBehaviour
    {
        [SerializeField]
        private int _attackPower = 10;

        [SerializeField]
        private float _knockbackPower = 1f;

        [SerializeField]
        private float _hitWaitTime = 0.8f;

        [SerializeField]
        private Transform _damageSourceTransform;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "EnemyCharacter")
            {
                var enemy = collision.GetComponent<EnemyCharacter>();
                var attackId = GetInstanceID();
                // もし直近で攻撃していれば、ヒットはさせない
                if(enemy.ImmuneCheck(attackId))
                {
                    return;
                }

                var uniqueId = GetInstanceID(); // このオブジェクトの一意なIDを取得
                // 連続ヒットを防ぐため、ヒット後0.4秒は当たらないよう登録いただく
                var damageInfo = new EnemyCharacter.DamageInfo(){AttackId = uniqueId, WaitTime = _hitWaitTime};
                enemy.OnAttacked(_attackPower, _knockbackPower, _damageSourceTransform.position, damageInfo);
            }
        }

        public void SetAttackPower(int power)
        {
            _attackPower = power;
        }

    }
}
