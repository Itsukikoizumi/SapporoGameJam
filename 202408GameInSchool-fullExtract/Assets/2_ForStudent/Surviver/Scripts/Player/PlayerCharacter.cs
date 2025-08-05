namespace Surviver
{
    using DG.Tweening;
    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// プレイヤーキャラクターを扱うクラス。
    /// 操作で動いたり、敵に攻撃されたり
    /// </summary>
    public class PlayerCharacter : MonoBehaviour
    {
        // グラフィック系パラメータ
        [SerializeField]
        private SpriteRenderer _charaSprite;
        [SerializeField]
        private Shaker _shaker;
        [SerializeField]
        private TextMeshProUGUI _hpText;
        private SurviverSceneUI _sceneUI;

        // 移動系パラメータ

        /// <summary>
        /// 通常時の速度
        /// </summary>
        [SerializeField]
        private float normalSpeed = 1.0f;

        /// <summary>
        /// 加速時の速度
        /// </summary>
        [SerializeField]
        private float boostedSpeed = 2.0f;

        /// <summary>
        /// 加速状態ならtrue
        /// </summary>
        private bool boosted => _boostTime > 0f;

        /// <summary>
        /// 加速状態の時間
        /// </summary>
        private float _boostTime;

        private Rigidbody2D _rigidbody;

        // Hp系パラメータ
        [SerializeField]
        public int MaxHp { get; private set;} = 100;
        [SerializeField]
        public int CurrentHp { get; private set;} = 100;

        private bool _isAlive = true; // 生存フラグ
        private float _invincibleTime = 0; // 無敵時間

        // 所持アイテム系パラメータ
        [SerializeField]
        private WeaponHolder _weaponHolder;
        public float ItemGetRange { get; private set; } = 3f;
        private int _level = 1;
        private int _exp = 0;
        private int _nextLvExp = 0;
        private int _preNextLvExp = 0;
        private int _baseNextLvExp = 8;

        private Item Barrier;
        private Item bat;
        private Item Boots;
        private Item stun;

        public void Initialize(SurviverSceneUI ui)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _sceneUI = ui;
            _weaponHolder.Initialize();
        }

        public void ResetStatus()
        {
            // 位置をもとに戻して
            transform.localPosition = Vector3.zero;

            // HPも最大に
            CurrentHp = MaxHp;
            _isAlive = true;
            _charaSprite.transform.rotation = Quaternion.Euler(0, 0, 0);

            // 経験値リセット
            _exp = 0;
            _nextLvExp = _baseNextLvExp;
            _preNextLvExp = 0;
            _level = 1;

            // 武器状態リセット
            _weaponHolder.ResetStatus();
            _weaponHolder.SetLevel(0);
            // デフォの武器だけonに
            _weaponHolder.SetLv1DefaultWeapon();

            // 加速状態をリセット
            _boostTime = 0f;

            RefreshUI();
            RefreshExpGauge();
        }

        public void OnUpdate()
        {
            if(_isAlive)
            {
                Move();
                _weaponHolder.OnUpdate();
            }
            InvinsibleCountDown();
            BoostTimeCountDown();
        }
        // ---------------

        /// <summary>
        /// 敵から攻撃されたときの関数
        /// </summary>
        /// <param name="damageValue"> ダメージ値 </param>
        /// <param name="damageSourcePos"> ダメージを与えてきたやつの場所 </param>
        public void OnAttacked(int damageValue, float knockbackPower, Vector3 damageSourcePos)
        {
            // 既に死んでいるか、無敵時間中なら何もしない
            if(!_isAlive || _invincibleTime > 0)
            {
                return;
            }

            // HPを減らす
            OnDamaged(damageValue);

            // 被ダメ時に身体をプルプル
            _shaker.Play();

            // ノックバック
            var knockBackDir = (transform.position - damageSourcePos).normalized;
            _rigidbody.AddForce(knockBackDir * knockbackPower, ForceMode2D.Impulse);
            
            // 無敵時間を設定
            _invincibleTime = 0.5f;
        }

        public void AddExp(int exp)
        {
            // todo: 経験値取得時に効果音
            _exp += exp;

            var preLevel = _level;

            if(LvUpCheck())
            {
                // レベルアップ
                _sceneUI.PlayLvUpEffect();
                _weaponHolder.OpenLvUpPopup(_level - preLevel);
            }

            RefreshExpGauge();
        }

        /// <summary>
        /// レベルアップしたかのチェックを再帰的に行います。
        /// </summary>
        private bool LvUpCheck()
        {
            if(_exp >= _nextLvExp)
            {   
                // レベルアップ
                _level++;
                _preNextLvExp = _nextLvExp;
                _nextLvExp += _baseNextLvExp + (_level * 5);
                LvUpCheck();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 操作を受け取って移動する
        /// </summary>
        private void Move()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            if(x == 0 && y == 0)
            {
                var moveInput = _sceneUI.GetMoveInput();
                x = moveInput.x;
                y = moveInput.y;
            }

            // ななめ移動したときに早くならないよう正規化。
            var moveDir = new Vector3(x, y, 0).normalized;

            // 速度を求めて設定
            var speed = boosted ? boostedSpeed : normalSpeed;
            _rigidbody.AddForce(moveDir * speed);
        }

        /// <summary>
        /// 無敵時間の経過を数え上げる
        /// </summary>
        private void InvinsibleCountDown()
        {
            if(_invincibleTime > 0)
            {
                _invincibleTime -= Time.deltaTime;
            }
        }

        /// <summary>
        /// 無敵時間の経過を数え上げる
        /// </summary>
        private void BoostTimeCountDown()
        {
            _boostTime -= Time.deltaTime;
        }

        private void OnDamaged(int damageValue)
        {
            // HPを減らす
            CurrentHp -= damageValue;
            RefreshUI();

            // HPが0になっていれば死亡
            if(CurrentHp <= 0)
            {
                CurrentHp = 0;
                _isAlive = false;
                _charaSprite.transform.DORotate(new Vector3(0, 0, 90), 0.5f);

                _sceneUI.SetGameOver(true);
            }
        }

        private void RefreshUI()
        {
            _hpText.text = $"HP: {CurrentHp} / {MaxHp}";
        }

        private void RefreshExpGauge()
        {
            // 次のレベルアップまでの経験値取得割合を計算
            var expProgress = Mathf.InverseLerp(_preNextLvExp, _nextLvExp, _exp);
            _sceneUI.SetExpGauge(expProgress);
        }

        private void Update()
        {
            //this.Barrier = GameObject.Find("Item Brrier");
            //this.bat = GameObject.Find("Item bat");
            //this.stun = GameObject.Find("Item stun");
            //this.Boots = GameObject.Find("Item Boots");
        }

        public void ItemAbility(Item.Kind kind)
        {
            switch (kind)
            {
                case Item.Kind.Boots:
                    _boostTime = 7f;
                    break;
            }
        }
    }
}