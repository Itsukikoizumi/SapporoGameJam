using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// 端から端に移動して、到達したらワープして繰り返す壁。
    /// </summary>
    public class MovingWall : MonoBehaviour
    {
        [Header("画面左の外に出た時のx座標")]
        [SerializeField]
        private float _endPointX;

        [Header("ランダムな高さの内一番高いところのy座標")]
        [SerializeField]
        private float _maxY;

        [Header("ランダムな高さの内一番低いところのy座標")]
        [SerializeField]
        private float _minY;

        [SerializeField]
        private float _speed = 3f;

        // Update is called once per frame
        void Update()
        {
            if (transform.localPosition.x <= _endPointX)
                gameObject.SetActive(false);　// 画面外に出たら非表示にする

            var offset = Vector3.left * _speed * Time.deltaTime;

            // todo: スコアで壁の速度変えてだんだん難しくするなど
            // offset += offset * (0.1f * ScoreDataManager.Instance.Score);
            

            transform.localPosition += offset;
        }

        /// <summary>
        /// ランダムな高さに設定する
        /// </summary>
        public void SetRandomHeight()
        {
            float randomY = Random.Range(_minY, _maxY);

            transform.localPosition += new Vector3(0, randomY, 0);
        }
    }
}