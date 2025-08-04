#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class AudioLoaderEditor : EditorWindow
{
    private string bgmFolderPath = "Audio/BGM"; // Specify the folder path here
    private string seFolderPath = "Audio/SE"; // Specify the folder path here

    [MenuItem("Window/Audio Loader")]
    public static void ShowWindow()
    {
        GetWindow<AudioLoaderEditor>("Audio Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Audio Loader", EditorStyles.boldLabel);
        GUILayout.Label("Resourceフォルダ以下のフォルダパスを指定してください");

        bgmFolderPath = EditorGUILayout.TextField("bgm Folder Path", bgmFolderPath);
        seFolderPath = EditorGUILayout.TextField("se Folder Path", seFolderPath);

        if (GUILayout.Button("Load Audio Clips"))
        {
            LoadAudioClips();
        }
    }

    private void LoadAudioClips()
    {
        AudioLoader audioLoader = FindObjectOfType<AudioLoader>();

        if (audioLoader == null)
        {
            Debug.LogError("AudioLoader script not found in the scene!");
            return;
        }

        {
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>(bgmFolderPath);
            Debug.Log(audioClips.Length);
            audioLoader.SetBGMs(audioClips.Select(clip => new AudioLoader.ClipPair(){ Key = clip.name, Clip = clip }).ToList());
        }

        {
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>(seFolderPath);
            audioLoader.SetSEs(audioClips.Select(clip => new AudioLoader.ClipPair(){ Key = clip.name, Clip = clip }).ToList());
        }


        Debug.Log("Audio clips loaded successfully!");
    }
}
#endif