using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    [field: SerializeField] private Animator animator;
    
    VideoPlayer _videoPlayer;
    
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    private void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    public void PlayFadeInAnimation()
    {
        animator.SetTrigger(FadeIn);
    }
    
    public void PlayFadeOutAnimation()
    {
        animator.SetTrigger(FadeOut);
    }
    
    //Write a function that waits for an animation, that is given as a parameter, to finish
    public IEnumerator WaitForAnimation(string animationName)
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }
    }

    public void PlayVideo()
    {
        StartCoroutine(WaitForAnimation("FadeIn"));
        _videoPlayer.Play();
    }
    
    public void StopVideo()
    {
        _videoPlayer.Stop();
        StartCoroutine(WaitForAnimation("FadeOut"));
    }
    
    public void PauseVideo()
    {
        _videoPlayer.Pause();
    }
    
    public void ResumeVideo()
    {
        _videoPlayer.Play();
    }
}
