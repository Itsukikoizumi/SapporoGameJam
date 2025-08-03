using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
namespace SummonerWar
{
    public class SummonerWarSceneUI : MonoBehaviour
    {
        [Header("プレイヤー召喚士を指定")]
        [SerializeField]
        private PlayerSummoner _player;


        [Header("UI群")]
        [SerializeField]
        private TextMeshProUGUI _costText;
        [SerializeField]
        private Image _bombGaugeImage;
        [SerializeField]
        private Button _bombButton;
        [SerializeField]
        private LvUpButton _lvUpButton;
        [SerializeField]
        private GameObject _gameOverPanel;

        [SerializeField]
        private List<SummonerWarCardUI> _cards;

        public void Initialize()
        {
            _bombButton.onClick.RemoveAllListeners();
            _bombButton.onClick.AddListener(_player.TryFireBomb);
            foreach (var card in _cards)
            {
                card.Initialize(_player);
            }
            _lvUpButton.Init(_player);
            SetGameOverPanelActive(false);
        }

        public void OnUpdate()
        {
            _costText.text = $"コスト: {(int)_player.Cost}/{(int)_player.MaxCost}";
            _bombGaugeImage.fillAmount = _player.BombCooldownValue;
            _lvUpButton.OnUpdate();
        }

        public void OnResetStatus()
        {
            _lvUpButton.UpdateText();
        }

        public void SetGameOverPanelActive(bool isActive)
        {
            _gameOverPanel.SetActive(isActive);
        }
    }
}