using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace SummonerWar
{
    /// <summary>
    /// HP、攻撃力を持つプレイヤーユニット
    /// 敵と衝突するとダメージを与えつつ反撃を受ける 
    /// </summary>
    public class PlayerUnit : MonoBehaviour
    {
        public int MaxHp = 10;
        protected int _hp = 10;

        /// <summary>
        /// 攻撃力
        /// </summary>
        public int AttackPower = 2;

        /// <summary>
        /// 無敵か
        /// </summary>
        protected bool IsInvincible => _invincibleCount > 0f;
        protected float _invincibleCount = 0f;

        private UnitMove _unitMove;
        private PlayerSummoner _summoner;

        public void SetSummoner(PlayerSummoner summoner)
        {
            this._summoner = summoner;
        }

        private void Start()
        {
            _hp = MaxHp;
            _unitMove = GetComponent<UnitMove>();
        }
        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            // もし無敵状態なら無敵時間を減らす
            if (IsInvincible)
                _invincibleCount -= Time.deltaTime;
        }

        public virtual void GetDamage(int value, ref bool isDead, bool isPenetrateInvincible = false)
        {
            if (!isPenetrateInvincible && IsInvincible)
                return;

            _hp -= value;

            if (_hp <= 0)
            {
                // 撃破される
                // todo: 撃破時効果音 or エフェクト
                _summoner?.ForgetPet(this);
                isDead = true;
                Destroy(gameObject);
            }
            else
            {
                // 一定時間無敵
                _invincibleCount = 0.5f;

                isDead = false;
                // ノックバックする
                _unitMove.Knockback();
            }
        }

        public void KnockBack()
        {
            _unitMove.Knockback();
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
                enemyUnit.GetDamage(AttackPower, ref isDead);
            }
        }
    }
}