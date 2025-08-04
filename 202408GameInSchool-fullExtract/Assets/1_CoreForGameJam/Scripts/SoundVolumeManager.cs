using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeManager : MonoBehaviour
{
    public void SetMusicVolume(float volume)
    {
        Hellmade.Sound.EazySoundManager.GlobalMusicVolume = volume;
    }

    public void SetSEVolume(float volume)
    {
        Hellmade.Sound.EazySoundManager.GlobalSoundsVolume = volume;
        Hellmade.Sound.EazySoundManager.GlobalUISoundsVolume = volume;
    }
}
