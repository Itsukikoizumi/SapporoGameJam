using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
namespace Surviver
{
    /// <summary>
    /// 敵たちを管理するクラス
    /// </summary>
    public class EnemyCharacterPool : PoolBase<EnemyCharacter>
    {
        // poolBaseの方で敵をリストで持っています。

        [SerializeField]
        private float _spawnTimer = 3f;
        private float _currentSpawnTimer = 3f;
        [SerializeField]
        private int _spawnNum = 2;
        [SerializeField]
        private int _lvUpSpawnCount = 5;
        
        private int _currentSpawnCount = 0;

        /// <summary>
        /// ターゲットとなるプレイヤー
        /// </summary>
        private PlayerCharacter _playerCharacter;
        private ExpItemPool _expItemPool;
        private EnemyBulletPool _enemyBulletPool;

        private int Lv => 1 + (_currentSpawnCount / _lvUpSpawnCount);

        /// <summary>
        /// 初期化します。
        /// </summary>
        public void Initialize(PlayerCharacter playerCharacter, ExpItemPool expItemPool, EnemyBulletPool enemyBulletPool)
        {
            ClearAllEnemy();
            // 敵たちに狙わせるため、プレイヤー情報を保持
            _playerCharacter = playerCharacter;
            _expItemPool = expItemPool;
            _enemyBulletPool = enemyBulletPool;

            Prepare(10);
        }


        public void ClearAllEnemy()
        {
            ClearAll();
            _currentSpawnTimer = _spawnTimer;
        }

        public void ResetStatus()
        {
            HideAll();
            _currentSpawnTimer = _spawnTimer;
            _currentSpawnCount = 0;
        }


        /// <summary>
        /// 敵を動かしたり、定期的に敵を生成したり
        /// </summary>
        public void OnUpdate()
        {
            // フレームごとに処理してアクティブな敵たちを動かします。
            foreach (var e in _list)
            {
                if (e.gameObject.activeInHierarchy)
                {
                    e.OnUpdate();
                }
            }

            // スポーンタイマーを経過時間分減らして、満たしていそうなら生成
            _currentSpawnTimer -= Time.deltaTime;
            if(_currentSpawnTimer <= 0f)
            {
                SpawnEnemyAroundPlayer();
                _currentSpawnTimer = _spawnTimer;

                _currentSpawnCount++;
            }
        }

        /// <summary>
        /// 敵を出現させる。
        /// </summary>
        public void SpawnEnemy(Vector3 position)
        {
            // PoolBaseのCreateを呼び出して敵を作成
            var enemy = Create();

            enemy.ResetStatus(1 + (_currentSpawnCount / _lvUpSpawnCount));
            // 指定されたポジションにワープさせて
            enemy.transform.position = position;
            // 表示
            enemy.Show();
        }

        /// <summary>
        /// 敵を新規に作って返します。
        /// </summary>
        protected override EnemyCharacter CreateInternal()
        {
            // PoolBaseのCreateInternalを呼び出して、敵を作成
            var enemy = base.CreateInternal();

            // 初期化
            enemy.Initialize(_playerCharacter.transform, OnEnemyDown, _enemyBulletPool);
            return enemy;
        }

        /// <summary>
        /// プレイヤーの周りに敵を沸かす
        /// </summary>
        private void SpawnEnemyAroundPlayer()
        {
            for (var i = 0; i < _spawnNum + (Lv - 1); i++)
            {
                // やってることとしては {x: -1~1, y:-1~1のランダムなベクトルを用意したあと、正規化でベクトルの大きさを1にしたあと、距離をかけてます
                var randamPos = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f).normalized * 10f;
                // プレイヤーを中心として、少し離れたランダムな位置を用意
                SpawnEnemy(_playerCharacter.transform.localPosition + randamPos);
            }
        }

        // 敵がやられたとき
        private void OnEnemyDown(EnemyCharacter enemy)
        {
            for (var i = 0; i < enemy.Exp + (Lv / 2); i++)
            {
                // 敵がやられたら、経験値アイテムを出現させる
                _expItemPool.SpawnExp(enemy.transform.position);
            }
        }
    }
}