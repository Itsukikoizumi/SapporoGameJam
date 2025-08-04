＜ゲームパッケージについて＞
・FlappyBird
フラッピーバードです。クリックしたら鳥が羽ばたくので、土管に当たらないよう高度を調整するやつです。
一番構造が簡単。

・SummonerWar
にゃんこ大戦争みたいな一次元タワーディフェンスです。自然回復するコストを、自己強化かユニット召喚に割り振りつつ、相手を陥落させるやつです。
設計がちょっと雑かつ、少し難しい。

・Surviver
ヴァンパイアサバイバー、ダダサバイバーみたいなやつです。敵を倒しながら、レベルを武器に割り振って、激しくなる敵の猛攻を食い止めながら生き残るやつです。
設計はしっかり目に行ったものの、やはり難しい。

＜ゲーム画面として上記ゲームを追加するには＞
改造したいゲームPackを展開した後、中のxxxxxxxxSceneのprefabをHierarchy上に配置、
GameManager > SceneManagerのScenesに追加。

例えば3番目に追加した場合、0スタートなので添え字は2、
SceneManager.Instance.ChangeSceneWithFade(2);
で画面遷移ができます。
TitleScene.cs内のOnNextScene()メソッドで呼ばれます


＜よくありそうな質問＞
※8/4～の演習中、分からないことがあれば基本的にメンターに聞いちゃってOKです。多分暇しています。
ソースコードとかprefabの構造とかも大体理由あって今の作りにしているので、設計思想とかも聞いちゃってください。


・BGM/SEを追加するには?
1. Assets/Resources/Audioフォルダの中に
　SEフォルダ
　BGMフォルダ
がありますので、そこにWavやmp3、oggを追加してください。
（ネット上の素材は大抵利用規約ありますので要確認。特にライセンス表記が必要かなど。
オススメは記入必要なしの効果音ラボ
https://soundeffect-lab.info/sound/button/）

2. Hierarchy上のGameManagerオブジェクトをクリック、
Inspector上のAudioLoaderコンポーネントの一番下にある「OpenAudioLoaderWindow」をクリック

3. 現れたウィンドウにて、
BGMPathを　Audio/BGM
SEPathを　Audio/SE
に設定したあと、
LoadAudioClipsをクリック

4. Inspector上のBgmDict、SeDictに追加したファイルがあることを確認。

5. EazySoundManager.PlayMusic(AudioLoader.BGM("魔法使いの旅路"));
EazySoundManager.PlayUISound(AudioLoader.SE("Accept"));

といった感じで、ファイル名を指定することで効果音の再生が可能になります。


---------------------------------

・まず何をすればいいかな

分かりやすく見た目が変わるのが画像/文言差し替えです。
タイトルの背景を変えてみたり、タイトル名を変えてみたり。
キャラクタのSpriteRendererで指定している画像を変えるなど。

次にオススメなのがBGM、SEの再生処理追加です。
ゲーム中で何が起こっているのかを書き留めておいて、そこに音を入れたければ処理を追っていく感じです。
例) プレイヤーが攻撃を食らったときに音を鳴らしたい > PlayerXXX.csに被ダメ時用の処理(OnDamaged、OnAttacked)がありそう or 敵(EnemyXXXX.cs)が攻撃したときの処理があるはず
関数の呼び出し元を見てみて、オブジェクト同士の関係を把握してみるなど
など...

上記でプログラムに慣れてきたら、数字を差し替えてゲームを難しく/優しくしてみたり、キャラやアイテム、武器などを追加してみるのも楽しいはずです。
UnityPackageを再度展開したらコードやprefabは元に戻せますので、恐れずに。

あまりに構造が複雑そうだったら、FlappyBirdを見てみるのも手
---------------
・上記についてさらに具体案が欲しい

VisualStudioなどでCtrl + Shift + Fキーで全ソースコードに検索を掛けることができるので、そこで「todo:」を探せば
なんかやれそうな箇所が見つかります。

また、unitypackage内に「改修案」テキストを残しています。、記載内容を優しい順に攻めていくなど。