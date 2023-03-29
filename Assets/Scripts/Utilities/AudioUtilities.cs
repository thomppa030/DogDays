using System;
using System.Collections;
using UnityEngine;

public class AudioUtilities : MonoBehaviour
{
    public static void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }
    
    public static AudioClip GetRandomClip(AudioClip[] AudioArray)
    {
        return AudioArray[UnityEngine.Random.Range(0, AudioArray.Length)];
    }
}