using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class Shaker : MonoBehaviour
{
    public void Play(float duration = 0.5f, float magnitude = 0.5f)
    {
        transform.localPosition = Vector3.zero;
        var value = magnitude;
        DOTween.To(
            () => magnitude,
            it =>
            {
                value = it;
                transform.localPosition = new Vector3(
                    Random.Range(-value, value),
                    Random.Range(-value, value),
                    0f
                );
            },
            0,
            duration)
            .OnComplete(() => transform.localPosition = Vector3.zero)
            .Play();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Shaker))]
public class HelpBoxTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("振動開始時、終了時にlocalPositionを0,0,0に変更(戻し)します。", MessageType.Warning);
    }
}
#endif