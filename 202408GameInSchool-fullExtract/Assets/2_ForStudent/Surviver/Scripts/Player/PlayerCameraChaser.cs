using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    public class PlayerCameraChaser : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private void Update()
        {
            var targetPos = _target.position;
            targetPos.z = -10;
            transform.position = Vector3.Lerp(transform.position, targetPos, 2f * Time.deltaTime);
        }
    }
}
