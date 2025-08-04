using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Surviver
{
    public class PlayerBulletBomb : PlayerBullet
    {
        [SerializeField]
        private GameObject _bulletImage;
        [SerializeField]
        private PlayerAttackArea _explosionEff;


        private Sequence _effectSequence;

        public override void Shoot(Vector3 position, Vector3 direction, float offsetAngle = 0)
        {
            base.Shoot(position, direction, offsetAngle);
            _explosionEff.gameObject.SetActive(false);
            _bulletImage.SetActive(true);

            _effectSequence?.Kill();
        }

        public override void LifeEnd()
        {
            _explosionEff.gameObject.SetActive(true);
            _bulletImage.SetActive(false);
            _explosionEff.transform.localScale = Vector3.zero;

            _effectSequence = DOTween.Sequence()
                .SetEase(Ease.OutQuad)
                .Append(_explosionEff.transform.DOScale(Vector3.one, 0.8f))
                .Append(_explosionEff.transform.DOScale(Vector3.zero, 0.2f))
                .OnComplete(() => base.LifeEnd())
                .Play();
        }
    }
}
