using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SceneManager側で管理される1画面単位のスーパークラス
/// </summary>
public class SceneBase : MonoBehaviour
{
    /// <summary>
    /// ゲーム開始時に画面を隠す処理
    /// </summary>
    public virtual void OnGameStart(){}

    /// <summary>
    /// シーンへの遷移タイミング時の処理
    /// </summary>
    public virtual void OnSceneStart(){}

    /// <summary>
    /// SceneManager側でフレーム毎に呼ばれる処理
    /// </summary>
    public virtual void OnUpdate(){}

    /// <summary>
    /// シーンから離れる時の処理
    /// </summary>
    public virtual void OnSceneEnd(){}
}
