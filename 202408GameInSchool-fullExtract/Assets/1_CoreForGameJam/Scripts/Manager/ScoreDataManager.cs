using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム結果を保存するクラス
/// </summary>
public class ScoreDataManager : SingletonMonoBehaviour<ScoreDataManager>
{
    protected override void doAwake() { }

    /// <summary>
    /// 敵ボスを倒した数とか、生き残った時間とか、倒した数とか
    /// </summary>
    public int Score { get; private set; }

    public int HighScore { get; private set; }

    private List<ScoreText> _scoreTexts = new List<ScoreText>();

    /// <summary>
    /// 全プロパティを初期化します
    /// </summary>
    public void Reset()
    {
        SetScore(0);
    }

    public void AddScore(int value = 1)
    {
        Score++;
        CheckHighScore();
        RefreshScoreText();
    }

    public void SetScore(int value = 0)
    {
        Score = value;
        CheckHighScore();
        RefreshScoreText();
    }

    /// <summary>
    /// スコアを表示するテキストたちを登録します。
    /// </summary>
    /// <param name="t"></param>
    public void SetScoreText(ScoreText t)
    {
        _scoreTexts.Add(t);
    }

    /// <summary>
    /// スコア表示を更新します
    /// </summary>
    private void RefreshScoreText()
    {
        _scoreTexts.ForEach(scoreText => scoreText.Refresh());
    }

    private void CheckHighScore()
    {
        if (Score > HighScore)
            HighScore = Score;
    }
}