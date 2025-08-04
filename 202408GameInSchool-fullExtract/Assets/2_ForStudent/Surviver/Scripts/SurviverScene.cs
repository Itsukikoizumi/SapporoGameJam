using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// サバイバルゲームのシーン
    /// </summary>
    public class SurviverScene : SceneBase
    {
        // プレイヤーのキャラ
        [SerializeField]
        private PlayerCharacter _playerCharacter;

        // 敵たちの管理者
        // todo: ここを複数持たせてもいい
        [SerializeField]
        private List<EnemyCharacterPool> _enemyCharacterPools;

        // 経験値アイテムの管理者
        [SerializeField]
        private ExpItemPool _expItemPool;

        // 敵の弾の管理者
        [SerializeField]
        private EnemyBulletPool _enemyBulletPool;

        // UI管理者
        [SerializeField]
        private SurviverSceneUI _ui;

        /// <summary>
        /// 経過時間
        /// </summary>
        private float _spentTime = 0f;

        /// <summary>
        /// ゲーム開始時に画面を隠す処理
        /// </summary>
        public override void OnGameStart()
        {
            gameObject.SetActive(false);
            _ui.Initialize();
            _playerCharacter.Initialize(_ui);
            _enemyCharacterPools.ForEach(p => p.Initialize(_playerCharacter, _expItemPool, _enemyBulletPool));
            _expItemPool.Initialize(_playerCharacter);
            _enemyBulletPool.Prepare(10);
        }

        /// <summary>
        /// シーンへの遷移タイミング時の処理
        /// </summary>
        public override void OnSceneStart()
        {
            gameObject.SetActive(true);
            _ui.SetGameOver(false);

            // 重力を0にする
            Physics2D.gravity = Vector3.zero;

            _playerCharacter.ResetStatus();
            _enemyCharacterPools.ForEach(p =>
            {
                p.ResetStatus();
                p.HideAll();
            });
            _expItemPool.HideAll();
            _enemyBulletPool.HideAll();

            // 経過時間を0に
            _spentTime = 0f;
        }

        /// <summary>
        /// SceneManager側でフレーム毎に呼ばれる処理
        /// </summary>
        public override void OnUpdate()
        {
            _playerCharacter.OnUpdate();
            _enemyCharacterPools.ForEach(p => p.OnUpdate());
            _enemyBulletPool.OnUpdate();
            _expItemPool.OnUpdate();


            // 時間を経過させる
            _spentTime += Time.deltaTime;
        }

        /// <summary>
        /// シーンから離れる時の処理
        /// </summary>
        public override void OnSceneEnd()
        {
            gameObject.SetActive(false);

            // 重力を元に戻す
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }
}
