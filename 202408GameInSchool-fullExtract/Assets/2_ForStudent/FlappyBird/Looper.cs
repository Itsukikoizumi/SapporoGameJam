using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
    /// <summary>
    /// 端から端に移動して、到達したらワープして繰り返すコンポーネント
    /// </summary>
    public class Looper : MonoBehaviour
    {
        [SerializeField]
        private float _endPointX;

        private Vector3 _startPoint;

        [SerializeField]
        private float _speed = 3f;

        private void Start()
        {
            // 最初の位置を憶える
            _startPoint = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.localPosition.x <= _endPointX)
                transform.localPosition = _startPoint;
            transform.localPosition += Vector3.left * _speed * Time.deltaTime;
        }
    }
}