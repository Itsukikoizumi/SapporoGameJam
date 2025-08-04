using UnityEngine;
namespace SummonerWar
{
    /// <summary>
    /// 敵のユニットの生成データ。 生成されるユニットと、スポーン間隔を設定できます。/// 
    /// </summary>
    [System.Serializable]
    public class EnemyUnitSpawnData
    {
        [Header("生成する敵")]
        public EnemyUnit EnemyUnitPrefab;
        [Header("何秒ごとに生成するか")]
        public float SpawnIntervalSeconds = 5f;

        private float _spawnTimer = 0f;

        public void ResetTimer()
        {
            _spawnTimer = SpawnIntervalSeconds;
        }

        /// <summary>
        /// 今スポーンタイミングか？
        /// (スポーンまでの時間カウントも兼ねています。) 
        /// </summary>
        public bool IsSpawnTiming()
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0f)
            {
                // タイマーをリセット
                ResetTimer();
                return true;
            }
            else
                return false;
        }
    }
}