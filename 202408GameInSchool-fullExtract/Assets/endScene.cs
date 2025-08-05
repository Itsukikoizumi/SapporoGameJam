using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using Surviver;
using UnityEngine;

public class endScene : SceneBase
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
        Debug.Log("エンドシーン開始");
        EazySoundManager.StopAllMusic();
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
    public void GotitleScene()
    {
        Debug.Log("タイトルへ");
        EazySoundManager.PlayUISound(AudioLoader.SE("Accept"));
        SceneManager.Instance.ChangeSceneWithFade(1);
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
