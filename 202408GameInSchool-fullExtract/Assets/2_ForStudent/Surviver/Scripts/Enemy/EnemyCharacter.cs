using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 敵キャラのクラスです。プレイヤーを追っかけたり、倒されたり。
    /// ※どのオブジェクトと衝突するかは、UnityのProjectSettings > Physics2Dで設定してください。
    /// </summary>
    public class EnemyCharacter : MonoBehaviour
    {
        [SerializeField]
        private int _attackPower = 10;

        // HP系変数
        [SerializeField]
        private int _maxHp = 3;
        private int _hp;

        // 移動系変数
        [SerializeField]
        private float _speed = 5f;

        [SerializeField]
        private Shaker _shaker;

        [SerializeField]
        private List<EnemyBulletPattern> _bulletPatterns = new List<EnemyBulletPattern>();

        public int Exp = 2;
        
        private Rigidbody2D _rigidbody;
        // ターゲットとなるプレイヤーの位置を持つクラス
        private Transform _playerTransform;

        public class DamageInfo
        {
            public int AttackId;
            public float WaitTime;
        }

        /// <summary>
        /// 連続ヒットを防ぐため、ここのリストにある攻撃に対しては無敵になる
        /// </summary>
        private List<DamageInfo> _immunes = new List<DamageInfo>();

        private Action<EnemyCharacter> _onEnemyDown;
        protected EnemyBulletPool _enemyBulletPool;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(Transform playerTransform, Action<EnemyCharacter> onEnemyDown, EnemyBulletPool enemyBulletPool)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerTransform = playerTransform;
            _onEnemyDown = onEnemyDown;
            _enemyBulletPool = enemyBulletPool;
            _bulletPatterns.ForEach(e => e.Initialize(playerTransform, enemyBulletPool));
        }

        public void ResetStatus(int level = 1)
        {
            _hp = _maxHp * level;
            _immunes = new List<DamageInfo>();
            _bulletPatterns.ForEach(e => e.ResetStatus(level));
        }

        public virtual void OnUpdate()
        {
            // 生きていれば...
            if(_hp > 0)
            {
                MoveToPlayer();
                _bulletPatterns.ForEach(e => e.OnUpdate());
            }

            // 無敵時間を減らす
            _immunes.ForEach((info) =>
            {
                info.WaitTime -= Time.deltaTime;
            });
            _immunes.RemoveAll(e => e.WaitTime <= 0);
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 表示する
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public bool ImmuneCheck(int attackId)
        {
            return _immunes.Any(e => e.AttackId == attackId);
        }

        public void OnAttacked(int damage, float knockBackPower, Vector3 damageSourcePos, DamageInfo damageInfo = null)
        {
            _hp -= damage;
            if(_hp <= 0)
            {
                // 倒された
                _onEnemyDown(this);

                // スコアアップ
                ScoreDataManager.Instance.AddScore(1);
                Hide();
            }
            else
            {
                // ノックバック
                var knockBackDir = (transform.position - damageSourcePos).normalized;
                _rigidbody.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);

                _shaker.Play(0.3f, 0.2f);

                if(damageInfo != null)
                {
                    _immunes.Add(damageInfo);
                }
            }
        }

        /// <summary>
        /// プレイヤーに向かって歩きます
        /// </summary>
        private void MoveToPlayer()
        {
            // プレイヤーの位置に向かうベクトルを取得
            var moveDir = (_playerTransform.position - this.transform.position).normalized;
            _rigidbody.AddForce(moveDir * _speed * Time.deltaTime * 100);
        }

        private void OnCollisionEnter2D(Collision2D collisionInfo)
        {
            // プレイヤーに当たったらダメージを与える
            if(collisionInfo.gameObject.tag == "PlayerCharacter")
            {
                var player = collisionInfo.gameObject.GetComponent<PlayerCharacter>();
                if(player)
                    player.OnAttacked(_attackPower, 5f, this.transform.position);

                // ノックバック
                var knockBackDir = (transform.position - player.transform.position).normalized;
                _rigidbody.AddForce(knockBackDir * 2f, ForceMode2D.Impulse);
            }
        }
    }
}