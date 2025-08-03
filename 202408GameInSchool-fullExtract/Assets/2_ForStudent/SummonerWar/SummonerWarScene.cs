using DG.Tweening;
using UnityEngine;

namespace SummonerWar
{
    /// <summary>
    /// にゃんこ〇戦争のような一次元タワーディフェンスシーンの管理
    /// </summary>
    public class SummonerWarScene : SceneBase
    {
        [SerializeField]
        private SummonerWarSceneUI _ui;

        [SerializeField]
        private PlayerSummoner _playerSummoner;

        [SerializeField]
        private EnemySummoner _enemySummoner;

        /// <summary>
        /// ゲーム開始時に画面を隠す処理
        /// </summary>
        public override void OnGameStart()
        {
            Hide();
        }

        /// <summary>
        /// シーンへの遷移タイミング時の処理
        /// </summary>
        public override void OnSceneStart()
        {
            Show();
            Debug.Log("にゃんこシーン開始");
            _playerSummoner.Initialize(OnPlayerDefeated);
            _enemySummoner.Initialize(OnEnemyDefeated);

            _ui.Initialize();

            // ボス撃破数のカウントをリセット
            ScoreDataManager.Instance.Reset();
        }

        /// <summary>
        /// SceneManager側でフレーム毎に呼ばれる処理
        /// </summary>
        public override void OnUpdate()
        {
            _ui.OnUpdate();
        }

        /// <summary>
        /// シーンから離れる時の処理
        /// </summary>
        public override void OnSceneEnd()
        {
            Hide();
        }


        /// private
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// プレイヤー敗北時の処理
        /// </summary>
        private void OnPlayerDefeated()
        {
            _ui.SetGameOverPanelActive(true);
            // 4秒待ってからタイトルへ
            DOVirtual.DelayedCall(4f, () =>
            {
                SceneManager.Instance.ChangeSceneWithFade(1);
            }).SetUpdate(true).Play();
        }

        /// <summary>
        /// 敵撃破時の処理
        /// </summary>
        private void OnEnemyDefeated()
        {
            // キルカウント増やす
            ScoreDataManager.Instance.AddScore();

            // 2秒待ってからプレイヤーと敵のステータスをリセットして開始
            DOVirtual.DelayedCall(2f, () =>
            {
                FadeManager.Instance.FadeInOut(0.3f, () =>
                {
                    _playerSummoner.ResetStatus();
                    _enemySummoner.ResetStatus();
                    _ui.OnResetStatus();
                });
            }).SetUpdate(true).Play();
        }
    }
}