using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SummonerWar
{
    /// <summary>
    /// プレイヤー。召喚士。
    /// </summary>
    public class PlayerSummoner : PlayerUnit
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private TextMeshProUGUI _headText;
        [SerializeField]
        private ParticleSystem _particle;
        [SerializeField]
        private Shaker _shaker;

        private List<PlayerUnit> _pets = new List<PlayerUnit>();

        // 召喚コスト系プロパティ
        public float MaxCost = 60f;
        public float Cost = 0f;
        public float CostPerSecond = 10f;
        public int Level = 1;

        private System.Action _onDefeat;

        // ボム系プロパティ
        [SerializeField]
        private PlayerBulletEmitter _bombEmitter;
        public float BombCooldownValue => _currentBombInterval / BombInterval;
        public float BombInterval = 10f;
        private float _currentBombInterval = 0f;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="onDefeat">やられたときに実行する処理</param>
        public void Initialize(System.Action onDefeat)
        {
            _onDefeat = onDefeat;
            ResetStatus();
        }

        public void ResetStatus()
        {
            _renderer.flipY = false;
            CleanPets();
            _hp = MaxHp;
            Level = 1;
            Cost = 0;
            _currentBombInterval = 0f;

            UpdateHeadText();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            // 死んでたらこの先の処理はしない
            if (_hp <= 0)
                return;

            // コスト回復
            if (Cost < MaxCost)
            {
                // レベルが上がるほど回復速度が上がる
                Cost += (CostPerSecond + Level * 2) * Time.deltaTime;
                if (Cost > MaxCost)
                    Cost = MaxCost;
            }

            // ボムのクールダウンが0以上ならカウントダウン
            if (_currentBombInterval > 0f)
            {
                _currentBombInterval -= Time.deltaTime;
                // todo: クールダウンが終わったら効果音とかエフェクト
                if (_currentBombInterval <= 0f)
                    Debug.Log("ボム発射可能");
            }
        }

        /// <summary>
        /// ダメージ食らった時の挙動。手下と同じ挙動だとDestroyされたりで困るのでoverride
        /// </summary>
        public override void GetDamage(int value, ref bool isDead, bool isPenetrateInvincible = false)
        {
            if (!isPenetrateInvincible && IsInvincible || _hp <= 0)
                return;

            _hp -= value;

            if (_hp <= 0)
            {
                // 撃破された
                _hp = 0;
                isDead = true;
                // ひっくり返る
                _renderer.flipY = true;
                // 初期化時に設定しておいた負けた時の処理を走らせる
                _onDefeat?.Invoke();
            }
            else
            {
                // 一定時間無敵
                _invincibleCount = 0.01f;
                isDead = false;
            }

            // 減らされたHpをUIに反映
            UpdateHeadText();

            // 体を揺らす
            _shaker.Play();
        }

        public void ForgetPet(PlayerUnit unit)
        {
            _pets.Remove(unit);
        }

        public void TrySummonPet(PlayerUnit prefab, float petCost)
        {
            if (Cost < petCost)
                return;

            // コストを消費
            Cost -= petCost;

            // ペットを召喚
            var pet = Instantiate(prefab, transform.position, Quaternion.identity);
            pet.SetSummoner(this);
            pet.transform.SetParent(transform.parent);
            _pets.Add(pet);

            _particle.Play();
        }

        /// <summary>
        /// ボムがクールダウン中でなければ発射
        /// </summary>
        public void TryFireBomb()
        {
            if (_currentBombInterval > 0f)
                return;

            _bombEmitter.CreateBullet();
            _currentBombInterval = BombInterval;
        }

        private void CleanPets()
        {
            // 召喚したペットをすべて削除
            foreach (var pet in _pets)
            {
                Destroy(pet.gameObject);
            }
            _pets.Clear();
        }

        private void UpdateHeadText()
        {
            _headText.text = $"あなた {_hp}/{MaxHp}";
        }
    }
}