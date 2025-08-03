using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// プレイヤーが触れた時にスコアが加算されるエリア
    /// </summary>
    public class ScoreArea : MonoBehaviour
    {
        /// <summary>
        /// RigidBody2dのIsTriggerがオンだとOnCollisionEnter2DではなくOnTriggerEnter2Dが呼ばれる。
        /// エネルギー弾とかレーザーとかセンサーとか、「ぶつかり」が発生しないものはIsTriggerがon。
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // もしプレイヤータグのオブジェクトが触れたら
            if (collision.CompareTag("Player"))
            {
                // 一応生きているかチェック。生きてなかったらスコアは加算しない
                var isAlive = collision.GetComponent<Player>().IsAlive;
                if (!isAlive)
                    return;

                // スコアを加算する
                ScoreDataManager.Instance.AddScore(1);

                // todo: スコアが上がったときに難易度を上げると面白くなるかも？
                //Time.timeScale += 0.1f;

                // todo: 効果音も鳴らす
                //EazySoundManager.PlaySound(AudioLoader.SE("Accept"));
            }
        }
    }
}