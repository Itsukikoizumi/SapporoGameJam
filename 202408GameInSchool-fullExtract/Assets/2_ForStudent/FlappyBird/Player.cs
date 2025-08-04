using Hellmade.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// フラッピーバードのプレイヤーキャラです
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Header("ジャンプ時の速度")]
        public float JumpVelocity = 3f;

        /// <summary>
        /// 物理挙動を担うコンポーネント
        /// </summary>
        private Rigidbody2D _rigidbody2D;

        /// <summary>
        /// 壁にぶつかったときの処理を登録するやつ
        /// </summary>
        private Action _onGameOver;

        /// <summary>
        /// 生きているか
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// 開始時の処理
        /// </summary>
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// 初期化処理。ゲームオーバー時の処理をFlappyBirdSceneから受け取ります
        /// </summary>
        public void Initialize(Action onGameOver)
        {
            _onGameOver = onGameOver;
        }

        /// <summary>
        /// 1フレーム(60fpsなら0.0166秒)ごとに呼ばれる処理
        /// </summary>
        void Update()
        {
            // もしすでに動けなかったらearly return。この先の処理を呼ばない
            if (!IsAlive) return;

            // クリック　もしくは スペースキーが押されたら
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                // 上方向に速度がかかっている状態に上書き。すなわち羽ばたき
                _rigidbody2D.velocity = Vector2.up * JumpVelocity;

                //todo: 効果音を鳴らす
                // EazySoundManager.PlaySound(AudioLoader.SE("Click"));
            }
        }

        /// <summary>
        /// 衝突判定。Rigidbody2D+Collider2Dを持つオブジェクトが、ほかのColider2Dを持つオブジェクトと衝突したときに呼ばれる
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 既にゲームオーバー済みなら何もしない
            if (!IsAlive)
                return;

            // 壁とタグ付けされているオブジェクトにぶつかればゲームオーバー
            if (collision.collider.CompareTag("Wall"))
            {
                // デバッグログを出力
                Debug.Log("Game Over");

                // 生きているフラグをfalseにして、ゲームオーバー処理を呼び出す
                IsAlive = false;
                _onGameOver();

                // todo: 効果音を鳴らす
                //EazySoundManager.PlaySound(AudioLoader.SE("death"));
            }
        }
    }
}
