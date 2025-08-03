using FlappyBird;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace FlappyBird
{
    /// <summary>
    /// 壁を用意するクラス。壁たちを持っておいて、非表示のものを使いまわす
    /// </summary>
    public class MovingWallSpawner : MonoBehaviour
    {
        private List<MovingWall> _movingWalls = new List<MovingWall>();

        // Start is called before the first frame update
        void Awake()
        {
            // 子となっているオブジェクトからmovingWallを取得
            _movingWalls = GetComponentsInChildren<MovingWall>(true).ToList();
        }

        /// <summary>
        /// 壁を用意する。
        /// </summary>
        public void SpawnWall()
        {
            // 使われていない壁を探す
            var wall = _movingWalls.FirstOrDefault(w => !w.gameObject.activeSelf);

            if (wall == null)
            {
                // 無ければ作る
                wall = Instantiate(_movingWalls[0], transform);
                _movingWalls.Add(wall);
            }

            // 自分の位置に壁を移動。壁はSpawnerの子に配置しているので0,0,0を渡せば位置が一緒になる
            wall.transform.localPosition = Vector3.zero;

            // 高さ変える
            wall.SetRandomHeight();

            // 表示
            wall.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            foreach(var wall in _movingWalls)
            {
                wall.gameObject.SetActive(false);
            }
        }
    }
}
