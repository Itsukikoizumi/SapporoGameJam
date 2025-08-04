using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// AudioClipを呼び出すクラス
/// </summary>
public class AudioLoader : SingletonMonoBehaviour<AudioLoader>
{
    [Serializable]
    public class ClipPair
    {
        // 音の名
        public string Key;
        // 音
        public AudioClip Clip;
    }

    [SerializeField]
    private List<ClipPair> _bgmDict = new List<ClipPair>();

    [SerializeField]
    private List<ClipPair> _seDict = new List<ClipPair>();

    protected override void doAwake()
    {
    }

    public static AudioClip BGM(string key) => Instance.GetBGMClip(key);
    public static AudioClip SE(string key) => Instance.GetSEClip(key);

    public AudioClip GetBGMClip(string key) => _bgmDict.FirstOrDefault(p => p.Key == key).Clip;
    public AudioClip GetSEClip(string key) => _seDict.FirstOrDefault(p => p.Key == key).Clip;

    public void SetBGMs(List<ClipPair> bgmDict)
    {
        _bgmDict = bgmDict;
    }

    public void SetSEs(List<ClipPair> seDict)
    {
        _seDict = seDict;
    }
}
