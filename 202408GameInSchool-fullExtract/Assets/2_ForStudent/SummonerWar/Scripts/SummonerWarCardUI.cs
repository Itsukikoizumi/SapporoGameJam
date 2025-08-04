using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SummonerWar
{
    public class SummonerWarCardUI : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private TextMeshProUGUI _costText;
        [SerializeField]
        private GameObject _disableFade;

        [Header("召喚するユニットを指定")]
        public PlayerUnit PlayerUnitPrefab;

        public float Cost;

        private PlayerSummoner _summoner;

        private bool _isInitialized = false;

        private void Start()
        {
            UpdateCostText();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            if (_disableFade.activeSelf && _summoner.Cost >= Cost)
            {
                _disableFade.SetActive(false);
            }
            else if (!_disableFade.activeSelf && _summoner.Cost < Cost)
            {
                _disableFade.SetActive(true);
            }
        }

        public void Initialize(PlayerSummoner summoner)
        {
            _summoner = summoner;
            _isInitialized = true;
        }

        private void UpdateCostText()
        {
            _costText.text = "コスト: " + Cost.ToString();
        }

        private void OnButtonClick()
        {
            _summoner.TrySummonPet(PlayerUnitPrefab, Cost);
        }
    }
}