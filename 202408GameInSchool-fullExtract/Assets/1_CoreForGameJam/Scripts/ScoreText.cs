using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ScoreText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private bool _isHighScore = false;

    public void Awake()
    {
        ScoreDataManager.Instance.SetScoreText(this);
        Refresh();
    }

    public void Refresh()
    {
        var score = _isHighScore ? ScoreDataManager.Instance.HighScore : ScoreDataManager.Instance.Score;
        _scoreText.text = $"{score}";
    }
}