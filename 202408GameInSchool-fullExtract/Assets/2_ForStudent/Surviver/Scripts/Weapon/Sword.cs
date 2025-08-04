using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    /// <summary>
    /// 回転する剣...の一本の剣
    /// </summary>
    public class Sword : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttackArea _attackArea;

        public void SetAttackPower(int power)
        {
            _attackArea.SetAttackPower(power);
        }
    }
}
