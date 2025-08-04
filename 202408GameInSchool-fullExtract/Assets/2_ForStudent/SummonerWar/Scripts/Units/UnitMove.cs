using UnityEngine;

namespace SummonerWar
{
    /// <summary>
    /// ユニットの移動を祭るクラス
    /// </summary>
    public class UnitMove : MonoBehaviour
    {
        public enum Type
        {
            Player,
            Enemy
        }

        [Header("移動速度")]
        public float moveForce = 100f; // Adjust the force to control the speed of movement
        [Header("ノックバックの力")]
        public float knockBackForce = 2f;
        public float knockUpForce = 1f;
        [Header("味方か敵か")]
        public Type type; // 敵か味方か
        private Rigidbody2D rb;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// 進行方向とは逆方向にノックバックする
        /// </summary>
        public void Knockback(float rate = 1f)
        {
            // todo: 引数に応じてノックバックを強くする

            var force = knockBackForce;

            if (type == Type.Player)
            {
                rb.AddForce(Vector2.right * force + Vector2.up * knockUpForce, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.left * force + Vector2.up * knockUpForce, ForceMode2D.Impulse);
            }
        }

        private void Update()
        {
            if (type == Type.Player)
            {
                rb.AddForce(Vector2.left * moveForce * Time.deltaTime);
            }
            else
            {
                rb.AddForce(Vector2.right * moveForce * Time.deltaTime);
            }
        }
    }
}