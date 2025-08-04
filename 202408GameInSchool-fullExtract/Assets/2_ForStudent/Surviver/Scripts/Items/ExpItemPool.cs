using UnityEngine;
namespace Surviver
{
    public class ExpItemPool : PoolBase<ExpItem>
    {
        // poolBaseの方で経験値アイテムをリストで持っています。
        /// <summary>
        /// ターゲットとなるプレイヤー
        /// </summary>
        private PlayerCharacter _playerCharacter;

        /// <summary>
        /// 初期化します。
        /// </summary>
        public void Initialize(PlayerCharacter playerCharacter)
        {
            ClearAll();
            _playerCharacter = playerCharacter;

            // 画面に10個ぐらい経験値アイテム出す予定なので、あらかじめ生成
            Prepare(10);
        }

        /// <summary>
        /// 動かします
        /// </summary>
        public void OnUpdate()
        {
            // フレームごとに処理してアイテムを動かします
            foreach (var e in _list)
            {
                if (e.gameObject.activeInHierarchy)
                {
                    e.OnUpdate();
                }
            }
        }

        /// <summary>
        /// アイテムを出現させる。
        /// </summary>
        public void SpawnExp(Vector3 position)
        {
            var item = Create();
            item.ResetStatus();
            item.gameObject.SetActive(true);

            item.transform.localPosition = position;
        }

        protected override ExpItem CreateInternal()
        {
            // PoolBaseのCreateInternalを呼び出し
            var item = base.CreateInternal();

            // 初期化
            item.Initialize(_playerCharacter);
            return item;
        }
    }
}