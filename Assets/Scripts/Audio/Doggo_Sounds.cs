using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doggo_Sounds : MonoBehaviour
{

    [SerializeField]
    private AudioClip[] StepClips; 
    [SerializeField]
    private AudioClip[] BarkClips;
    [SerializeField]
    private AudioClip scratch; 
    [SerializeField]
    private AudioClip[] WhineClips;
    [SerializeField]
    private AudioClip[] SniffClips;
    

    private AudioSource audioSource;
    private Animator animation;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
    }

   /* public void Step()
    {
        if (animation.GetFloat("Speed_f") > 0.1 && animation.GetFloat("Speed_f") < 0.6)
        {
            AudioClip clip = GetRandomClip(StepClips);
            audioSource.PlayOneShot(clip);
        }
        
    }

    public void StepRun()
    {
        if (animation.GetFloat("Speed_f") >= 0.6)
        {
            AudioClip clip = GetRandomClip(StepClips);
            audioSource.PlayOneShot(clip);
        }
    }*/
    
    public void Bark()
    {
        AudioClip clip = GetRandomClip(BarkClips);
            audioSource.PlayOneShot(clip);
        
    }

    public void Scratching()
    {
        audioSource.PlayOneShot(scratch);
    }

    public void Whining()
    {
        AudioClip clip = GetRandomClip(WhineClips);
        audioSource.PlayOneShot(clip);
    }
    
    public void Sniff()
    {
        AudioClip clip = GetRandomClip(SniffClips);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip(AudioClip[] AudioArray)
    {
        return AudioArray[UnityEngine.Random.Range(0, AudioArray.Length)];
    }
}
