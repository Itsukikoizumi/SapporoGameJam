using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 画面切り替え周りの処理
/// </summary>
public class SceneManager : SingletonMonoBehaviour<SceneManager>
{
    [SerializeField]
    private SceneBase[] _sceneList;

    private SceneBase _currentScene = null;

    // lifecycle
    protected override void doAwake()
    {
        // 全画面初期化
        foreach (var scene in _sceneList)
            scene.OnGameStart();

        // 最初のシーンを指定
        ChangeScene(0);
    }

    private void Update()
    {
        _currentScene?.OnUpdate();
    }
    // public

    /// <summary>
    /// シーンを切り替える
    /// </summary>
    public void ChangeScene(int index)
    {
        _currentScene?.OnSceneEnd();
        _currentScene = _sceneList[index];
        _currentScene.OnSceneStart();
    }

    /// <summary>
    /// フェード付きでシーンを切り替える
    /// </summary>
    public void ChangeSceneWithFade(int index)
    {
        // フェードを動かすために、時間を進める
        Time.timeScale = 1f;

        InputManager.Instance.BlockInput(InputManager.BlockType.SceneManager);
        FadeManager.Instance.FadeInOut(0.3f, () =>
        {
            ChangeScene(index);
        }, () =>
        {
            InputManager.Instance.UnblockInput(InputManager.BlockType.SceneManager);
        });
    }
    public void OnNextScene()
    {
        int currentIndex = Array.IndexOf(_sceneList, _currentScene);
        int nextIndex = currentIndex + 1;

        if (nextIndex < _sceneList.Length)
        {
            ChangeSceneWithFade(nextIndex);
        }
        else
        {
            Debug.Log("これ以上シーンがありません");
            ChangeSceneWithFade(0);
        }
    }
}
