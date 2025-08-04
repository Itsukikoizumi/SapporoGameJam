using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SummonerWar
{
    public class EnemyUnitSpawnDataSet : MonoBehaviour
    {
        [System.Serializable]
        public class EnemyUnitSpawnDataByLevel
        {
            public int Level;
            public List<EnemyUnitSpawnData> SpawnDataList = new List<EnemyUnitSpawnData>();
        }

        [SerializeField]
        private List<EnemyUnitSpawnDataByLevel> _spawnDataList = new List<EnemyUnitSpawnDataByLevel>();

        /// <summary>
        /// 引数にレベルを渡すことで、そのレベルに沿って敵の出現データが返されます。
        /// </summary>
        public List<EnemyUnitSpawnData> GetSpawnDataSet(int level)
        {
            var data = _spawnDataList.FirstOrDefault(d => d.Level == level);

            // 指定レベルが見つからなかったら、今ある中で最大のレベルの出現データを出します
            if (data == null)
                data = _spawnDataList.OrderByDescending(d => d.Level).FirstOrDefault();

            return data.SpawnDataList;
        }
    }
}
