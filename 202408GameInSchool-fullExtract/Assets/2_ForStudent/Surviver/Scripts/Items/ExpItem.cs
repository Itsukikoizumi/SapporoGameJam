using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 経験値アイテムのクラスです
    /// </summary>
    public class ExpItem : MonoBehaviour
    {
        // ターゲットとなるプレイヤーの位置を持つクラス
        // todo: もし複数プレイヤーがいる場合、システム作り直しが必要...
        private Transform _playerTransform;
        private PlayerCharacter _playerCharacter;
        private float _lifeTime = 60f;
        private float _getProgress = 0f;

        private float _uncatchableCount = 0.1f;
        private bool _isGet = false;

        private Vector3 _moveDir;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(PlayerCharacter player)
        {
            _playerTransform = player.transform;
            _playerCharacter = player;
        }

        public void ResetStatus()
        {
            _isGet = false;
            _uncatchableCount = 0.1f;
            _lifeTime = 60f;
            _getProgress = 0f;
            _moveDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }

        public void OnUpdate()
        {
            if(_uncatchableCount > 0)
            {
                // 出現してから少しだけ移動する
                _uncatchableCount -= Time.deltaTime;
                transform.position += _moveDir * Time.deltaTime * 3f;
                return;
            }

            if(!_isGet)
            {
                if(IsCloseToPlayer(_playerCharacter.ItemGetRange))
                    _isGet = true;
                
                // 寿命処理
                _lifeTime -= Time.deltaTime;
                if(_lifeTime <= 0)
                {
                    Hide();
                }
            }
            else
            {
                // 取得範囲内に入ってればプレイヤーに向かって進む
                MoveToPlayer();

                if(IsCloseToPlayer(0.5f))
                {
                    _playerCharacter.AddExp(1);
                    Hide();
                }
            }
        }

        /// <summary>
        /// 非表示にする
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 表示する
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// プレイヤーに向かっていきます
        /// </summary>
        private void MoveToPlayer()
        {
            _getProgress = Mathf.Clamp01(_getProgress + Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _playerTransform.position, _getProgress * Time.timeScale);
        }

        private bool IsCloseToPlayer(float distance)
        {
            return Vector3.Distance(_playerTransform.position, transform.position) < distance;
        }
    }
}