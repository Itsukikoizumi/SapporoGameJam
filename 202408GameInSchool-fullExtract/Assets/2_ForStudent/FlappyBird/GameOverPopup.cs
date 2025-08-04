using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird
{
    /// <summary>
    /// ゲームオーバー時に表示されるポップアップです
    /// </summary>
    public class GameOverPopup : MonoBehaviour
    {
        /// <summary>
        /// リトライボタン
        /// </summary>
        [SerializeField]
        private Button _retry;
        
        /// <summary>
        /// タイトルに行くボタン
        /// </summary>
        [SerializeField]
        private Button _toTitle;

        /// <summary>
        /// 初期化処理です。FlappyBirdSceneからゲームリトライ処理やタイトルに戻る処理を受け取ります
        /// </summary>
        /// <param name="onRetry">ゲームリトライの処理です</param>
        /// <param name="onToTitle">タイトルに行く処理です</param>
        public void Initialize(Action onRetry, Action onToTitle)
        {
            // ボタンたちにそれぞれの処理を登録します
            _retry.onClick.AddListener(() => onRetry());
            _toTitle.onClick.AddListener(() => onToTitle());
        }

        private void Update()
        {
            // キーボード上のRキーを押してもリトライ
            if(Input.GetKeyDown(KeyCode.R))
            {
                _retry.onClick.Invoke();
            }
        }
    }
}
