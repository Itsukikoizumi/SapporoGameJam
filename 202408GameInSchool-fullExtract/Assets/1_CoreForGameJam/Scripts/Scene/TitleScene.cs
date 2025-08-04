using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using Surviver;
using UnityEngine;

public class TitleScene : SceneBase
{
    /// <summary>
    /// ゲーム開始時に画面を隠す処理
    /// </summary>
    public override void OnGameStart()
    {
        Hide();
    }

    /// <summary>
    /// シーンへの遷移タイミング時の処理
    /// </summary>
    public override void OnSceneStart()
    {
        Show();
        Debug.Log("タイトルシーン開始");
        EazySoundManager.StopAllMusic();
        EazySoundManager.PlayMusic(AudioLoader.BGM("魔法使いの旅路"));
    }

    /// <summary>
    /// SceneManager側でフレーム毎に呼ばれる処理
    /// </summary>
    public override void OnUpdate()
    {
    }

    /// <summary>
    /// シーンから離れる時の処理
    /// </summary>
    public override void OnSceneEnd()
    {
        Hide();
    }

    public void OnNextScene()
    {
        Debug.Log("ゲーム開始");
        EazySoundManager.PlayUISound(AudioLoader.SE("Accept"));
        SceneManager.Instance.ChangeSceneWithFade(2);
        EazySoundManager.StopAllMusic();
    }
    // private
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
