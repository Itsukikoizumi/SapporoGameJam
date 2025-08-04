#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioLoader))]
public class AudioLoaderInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open AudioLoader Window"))
        {
            AudioLoaderEditor.ShowWindow();
        }
    }
}
#endif