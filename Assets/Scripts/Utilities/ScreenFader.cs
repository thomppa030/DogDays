using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Animator fader;

    private void Start()
    {
        InteractionManager.Instance.OnFadeIn += FadeIn;
        InteractionManager.Instance.OnFadeOut += FadeOut;
    }

    IEnumerator PlayFadeAnimation(string animName)
    {
        fader.Play(animName);
        yield return new WaitForSeconds(1);
    }
    
    void FadeIn()
    {
        StartCoroutine(PlayFadeAnimation("FadeIn"));
    }
    
    void FadeOut()
    {
        StartCoroutine(PlayFadeAnimation("FadeOut"));
    }
}
