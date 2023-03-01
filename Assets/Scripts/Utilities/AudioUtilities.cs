using System.Collections;
using UnityEngine;

public class AudioUtilities : MonoBehaviour
{
    public static IEnumerator PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        yield return new WaitForSeconds(clip.length);
    }
    
    public static AudioClip GetRandomClip(AudioClip[] AudioArray)
    {
        return AudioArray[UnityEngine.Random.Range(0, AudioArray.Length)];
    }
}
