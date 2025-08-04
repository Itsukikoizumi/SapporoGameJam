using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// フラッピーバード画面を全体的に管理するクラスです
    /// </summary>
    public class FlappyBirdScene : SceneBase
    {
        [Header("プレイヤーキャラを登録してください")]
        [SerializeField]
        private Player _player;

        [Header("壁を管理するクラスを登録してください")]
        [SerializeField]
        private MovingWallSpawner _wallSpawner;

        [Header("ゲームオーバー画面を登録してください")]
        [SerializeField]
        private GameOverPopup _gameOverPopup;

        [Header("壁の出る間隔(秒)")]
        [SerializeField]
        private float _baseWallSpawnInterval = 1f;

        private float _wallSpawnInterval = 0f;

        private Vector3 _playerDefaultPosition;
        /// <summary>
        /// ゲーム起動時の処理
        /// </summary>
        public override void OnGameStart() 
        {
            gameObject.SetActive(false);

            // ゲーム起動時にオブジェクトの初期位置を保存
            _playerDefaultPosition = _player.transform.localPosition;

            // プレイヤーキャラにゲームオーバー時の処理を登録
            _player.Initialize(OnGameOver);

            // ゲームオーバーポップアップにリトライとタイトルへ戻る処理を登録
            _gameOverPopup.Initialize(
                OnRetry, 
                () => { 
                    // タイトルシーンのインデックスは1なので、1を渡して遷移
                    SceneManager.Instance.ChangeSceneWithFade(1); 
                });
        }

        /// <summary>
        /// シーンへの遷移タイミング時の処理
        /// </summary>
        public override void OnSceneStart() 
        {
            // 画面の表示
            gameObject.SetActive(true);

            // もしゲームオーバー画面が表示されていたら非表示にする
            _gameOverPopup.gameObject.SetActive(false);

            // オブジェクトを初期位置へ
            _player.transform.localPosition = _playerDefaultPosition;
            _wallSpawner.HideAll();
            // 次の壁を出すまでの時間を初期化。0ということはすぐ出る
            _wallSpawnInterval = 0f;

            // プレイヤーを生き返らせる
            _player.IsAlive = true;

            // 時間の進みをデフォルトにする
            Time.timeScale = 1f;

            // スコアをリセット
            ScoreDataManager.Instance.Reset();
        }

        /// <summary>
        /// SceneManager側でフレーム毎に呼ばれる処理
        /// </summary>
        public override void OnUpdate() 
        {
            _wallSpawnInterval -= Time.deltaTime;
            if(_wallSpawnInterval <= 0f)
            {
                SpawnWall();
                _wallSpawnInterval = _baseWallSpawnInterval;
            }
        }

        /// <summary>
        /// シーンから離れる時の処理
        /// </summary>
        public override void OnSceneEnd() 
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 壁を作ります
        /// </summary>
        private void SpawnWall()
        {
            _wallSpawner.SpawnWall();
        }

        /// <summary>
        /// ゲームオーバーの時の処理
        /// </summary>
        private void OnGameOver()
        {
            _gameOverPopup.gameObject.SetActive(true);

            // 一応壁とかは動かないように、時間を止める
            Time.timeScale = 0;

        }

        private void OnRetry()
        {
            _gameOverPopup.gameObject.SetActive(false);

            // フェードを動かすために、時間を進める
            Time.timeScale = 1f;

            // フェードアウトして、各位置をリセット。関数名はFadeInだが、これは黒画面をフェードインさせるということ。
            FadeManager.Instance.FadeIn(0.3f, () =>
            {
                // 画面に入った時と同じ処理をする。ゲームの再開。
                OnSceneStart();

                FadeManager.Instance.FadeOut(0.3f);
            });
        }
    }
}
