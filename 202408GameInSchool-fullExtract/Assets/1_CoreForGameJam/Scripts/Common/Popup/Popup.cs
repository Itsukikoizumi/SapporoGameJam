using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ポーズ画面とかの窓画面のことです。
/// </summary>
public class Popup : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [Header("出現時に拡縮させたいオブジェクト")]
    [SerializeField]
    private Transform _windowFrame;

    [SerializeField]
    private Button _closeButton;

    [Header("表示中、TimeScaleを0にするか(ポーズとかはOn)")]
    [SerializeField]
    private bool _isPause;

    private float _prePauseTimeScale;

    private bool _isPlaying = false;

    public System.Action OnClose;

    // 全部強制非表示
    public virtual void Initialize()
    {
        gameObject.SetActive(false);
        _canvasGroup.interactable = false;

        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() =>
            {
                Close();
            });
        }
    }

    public void Open()
    {
        SceneManager.Instance.StartCoroutine(WaitAnimation(OpenInternal));
    }

    private void OpenInternal()
    {
        _isPlaying = true;
        if (_isPause)
        {
            _prePauseTimeScale = Time.timeScale;
            // 時間を止める
            Time.timeScale = 0;
        }

        // ポップアップをクリックできるように
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _windowFrame.localScale = Vector3.zero;
        _canvasGroup.alpha = 0;

        gameObject.SetActive(true);
        _windowFrame.DOScale(Vector3.one, 0.2f) // 0.2秒掛けて1倍のサイズに拡大 
            .SetEase(Ease.OutQuad) // イージング。曲線的に動くぞ
            .SetUpdate(true) // timeScaleが0でも動くように
            .Play();

        _canvasGroup.DOFade(1, 0.2f)
            .SetUpdate(true)
            .OnComplete(() => _isPlaying = false)
            .Play();
    }

    public void Close()
    {
        SceneManager.Instance.StartCoroutine(WaitAnimation(CloseInternal));
    }

    private void CloseInternal()
    {
        _isPlaying = true;

        // ポップアップをクリックできないように
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _windowFrame.localScale = Vector3.one;
        _canvasGroup.alpha = 1;

        gameObject.SetActive(true);
        _windowFrame.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .Play();

        _canvasGroup.DOFade(0, 0.2f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                // フェードアウト時にオブジェクトを非表示にする
                if (_isPause)
                {
                    // 時間が進むようにする
                    Time.timeScale = 1f;
                }
                _isPlaying = false;
                OnClose?.Invoke();
                gameObject.SetActive(false);
            })
            .Play();
    }

    private IEnumerator WaitAnimation(Action action)
    {
        yield return new WaitUntil(() => !_isPlaying);
        action.Invoke();
    }
}
