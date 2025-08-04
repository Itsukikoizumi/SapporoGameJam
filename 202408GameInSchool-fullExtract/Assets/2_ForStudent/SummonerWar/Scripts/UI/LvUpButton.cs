using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Hellmade.Sound;
namespace SummonerWar
{
    /// <summary>
    /// ボタンを押すと召喚士のレベルが上がるボタン
    /// </summary>
    public class LvUpButton : MonoBehaviour
    {
        private static readonly int LvUpCost = 20;

        [SerializeField]
        private Button _buttonComponent;

        [SerializeField]
        private TextMeshProUGUI _lvText;

        [SerializeField]
        private TextMeshProUGUI _costText;

        [SerializeField]
        private GameObject _costOverIcon;

        [SerializeField]
        private CanvasGroup _lvUpDescText;

        private PlayerSummoner _player;
        private Sequence _lvUpEffectSequence;

        public void OnUpdate()
        {
            _costOverIcon.SetActive(_player.Cost < LvUpCost);
        }

        public void Init(PlayerSummoner player)
        {
            _player = player;
            UpdateText();
            _buttonComponent.onClick.RemoveAllListeners();
            _buttonComponent.onClick.AddListener(OnClick);
            _lvUpDescText.alpha = 0;
        }

        public void UpdateText()
        {
            _lvText.text = $"Lv {_player.Level}>{_player.Level + 1}";
            _costText.text = $"コスト: {LvUpCost}";
        }

        /// <summary>
        /// ボタンがクリックされたとき
        /// </summary>
        private void OnClick()
        {
            // コストが足りなければスルー
            if (_player.Cost < LvUpCost)
            {
                return;
            }

            _player.Cost -= LvUpCost;

            _player.Level++;
            UpdateText();
            PlayLvUpEffect();
        }


        private void PlayLvUpEffect()
        {
            EazySoundManager.PlayUISound(AudioLoader.SE("equiped"));
            // 前再生していたアニメをキャンセル
            _lvUpEffectSequence?.Kill();

            // まず文字をデカくして表示
            _lvUpDescText.transform.localScale = Vector3.one * 5f;
            _lvUpDescText.alpha = 1;

            // DoTweenによって徐々に小さくしつつ、最後に透明にする
            _lvUpEffectSequence = DOTween.Sequence()
                .Append(_lvUpDescText.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad))
                .AppendInterval(0.2f)
                .Append(_lvUpDescText.DOFade(0, 0.1f))
                .Play();
        }
    }
}