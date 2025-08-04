
// ポーズ画面です。
using Surviver;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : Popup
{
    [SerializeField]
    private Button _titleButton;

    public override void Initialize()
    {
        base.Initialize();

        _titleButton.onClick.RemoveAllListeners();
        _titleButton.onClick.AddListener(() =>
        {
            Close();
            SceneManager.Instance.ChangeSceneWithFade(1);            
        });
    }
}