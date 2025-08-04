using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace Surviver
{
    /// <summary>
    /// サバイバルゲームのシーンのUI
    /// </summary>
    public class SurviverSceneUI : MonoBehaviour
    {
        [SerializeField]
        private Image _expGaugeBar;
        private Tween _expGaugeBarTween;
        [SerializeField]
        private CanvasGroup _lvUpEff;
        [SerializeField]
        private GameObject _gameOverPanel;
        [SerializeField]
        private Joystick _joystick;

        public void Initialize()
        {
            _expGaugeBar.fillAmount = 0;
            _expGaugeBar.transform.localScale = Vector3.one;
            _lvUpEff.alpha = 0f;
        }

        public void SetExpGauge(float rate)
        {
            _expGaugeBar.transform.localScale = new Vector3(3, 1, 1);
            _expGaugeBarTween?.Kill();
            _expGaugeBarTween = _expGaugeBar.transform.DOScaleX(1, 0.3f).SetEase(Ease.OutQuad).SetUpdate(true).Play();
            _expGaugeBar.fillAmount = rate;
        }

        public void PlayLvUpEffect()
        {
            _lvUpEff.alpha = 1f;
            _lvUpEff.DOFade(0f, 0.5f).SetUpdate(true).Play();
        }

        public void SetGameOver(bool active)
        {
            _gameOverPanel.SetActive(active);
            if (!active)
                return;

            InputManager.Instance.BlockInput(InputManager.BlockType.GameOver);
            // 4秒待ってからタイトルへ
            DOVirtual.DelayedCall(4f, () =>
            {
                InputManager.Instance.UnblockInput(InputManager.BlockType.GameOver);
                SceneManager.Instance.ChangeSceneWithFade(1);
            }).SetUpdate(true).Play();
        }

        public Vector2 GetMoveInput()
        {
            return new Vector2(_joystick.Horizontal, _joystick.Vertical);
        }
    }
}

