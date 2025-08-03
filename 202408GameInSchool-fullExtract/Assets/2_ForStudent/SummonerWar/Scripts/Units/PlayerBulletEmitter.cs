using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SummonerWar
{
    /// <summary>
    /// PlayerBullet生成クラス
    /// </summary>
    public class PlayerBulletEmitter : MonoBehaviour
    {
        [SerializeField]
        private PlayerBullet _bulletPrefab;

        public void CreateBullet()
        {
            var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.SetParent(transform.parent);
        }
    }
}