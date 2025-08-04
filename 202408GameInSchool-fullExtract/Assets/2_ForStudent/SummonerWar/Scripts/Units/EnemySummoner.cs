using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace SummonerWar
{
    public class EnemySummoner : EnemyUnit
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private TextMeshProUGUI _headText;

        [SerializeField]
        private Shaker _shaker;
        [SerializeField]
        private EnemyUnitSpawnDataSet _spawnDataSet;
        
        private List<EnemyUnitSpawnData> _spawnDataList = new List<EnemyUnitSpawnData>();

        private List<EnemyUnit> _pets = new List<EnemyUnit>();
        private System.Action _onDefeat;

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
            
            UpdateHeadText();

            // todo: ゲーム進行に合わせて出現頻度や出現する敵を変更する。例えば引数にScoreDataManager.Instance.Score入れるとか
            _spawnDataList = _spawnDataSet.GetSpawnDataSet(1);

            foreach(var spawnData in _spawnDataList)
            {
                spawnData.ResetTimer();
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();


            // 死んでたらこの先の処理はしない
            if(_hp <= 0)
                return;

            // ペットのスポーン
            foreach(var spawnData in _spawnDataList)
            {
                if(spawnData.IsSpawnTiming())
                {
                    SpawnEnemy(spawnData);
                }
            }
        }

        public override void GetDamage(int value, ref bool isDead, bool isPenetrateInvincible = false)
        {
            if(!isPenetrateInvincible && IsInvincible || _hp <= 0)
                return;

            _hp -= value;

            if(_hp <= 0)
            {
                // 撃破された
                isDead = true;
                _hp = 0;

                // ひっくりかえる
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

        public void ForgetPet(EnemyUnit unit)
        {
            _pets.Remove(unit);
        }

        private void CleanPets()
        {
            // 召喚したペットをすべて削除
            foreach(var pet in _pets)
            {
                Destroy(pet.gameObject);
            }
            _pets.Clear();
        }

        private void UpdateHeadText()
        {
            _headText.text = $"敵ボス {_hp}/{MaxHp}";
        }

        private void SpawnEnemy(EnemyUnitSpawnData data)
        {
            var pet = Instantiate(data.EnemyUnitPrefab, transform.position, Quaternion.identity);
            pet.SetSummoner(this);
            pet.transform.SetParent(transform.parent);
            _pets.Add(pet);
        }
        
    }
}