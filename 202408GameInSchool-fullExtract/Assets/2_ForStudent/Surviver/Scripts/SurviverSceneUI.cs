using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
        [SerializeField]
        private float _timeLimit = 180f; // 3分
        private float _timer;
        private bool _timerActive = false;
        [SerializeField]
        private TextMeshProUGUI TimerText;

        private void Update()
        {
            if (!_timerActive)
                return;

            _timer -= Time.unscaledDeltaTime;

            // タイマーが0以下になったらゲームオーバー
            if (_timer <= 0f)
            {
                _timer = 0f;
                _timerActive = false;
                SetGameOver(true);
            }

            UpdateTimerUI();

        }
        public void Initialize()
        {
            _expGaugeBar.fillAmount = 0;
            _expGaugeBar.transform.localScale = Vector3.one;
            _lvUpEff.alpha = 0f;

            _timer = _timeLimit;
            _timerActive = true;
        }
        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(_timer / 60);
            int seconds = Mathf.FloorToInt(_timer % 60);
            TimerText.text = $"{minutes:00}:{seconds:00}";

            // 残り30秒以下なら赤色に、それ以外は白色に
            if (_timer <= 15f)
            {
                TimerText.color = Color.red;
            }
            else
            {
                TimerText.color = Color.black;
            }
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

            DOVirtual.DelayedCall(4f, () =>
            {
                InputManager.Instance.UnblockInput(InputManager.BlockType.GameOver);
                SceneManager.Instance.ChangeSceneWithFade(3);
            }).SetUpdate(true).Play();
        }

        public Vector2 GetMoveInput()
        {
            return new Vector2(_joystick.Horizontal, _joystick.Vertical);
        }

        /// <summary>
        /// タイマーの ON/OFF を切り替える
        /// </summary>
        public void ToggleTimer()
        {
            _timerActive = !_timerActive;
        }

        /// <summary>
        /// タイマーを強制的に停止する
        /// </summary>
        public void StopTimer(bool v)
        {
            _timerActive = false;
        }

        /// <summary>
        /// タイマーを再開する
        /// </summary>
        public void StartTimer(bool v)
        {
            _timerActive = true;
        }
        public void ResetTimer()
        {
            // タイマーを初期化
            _timer = _timeLimit;
            _timerActive = false;
            UpdateTimerUI(); // UIも更新
        }
    }
}

