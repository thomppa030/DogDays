using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark
{
    private PlayerStateMachine playerStateMachine;
    private Animator anim;
    private AnimationClip Barkanim;

    public Bark(PlayerStateMachine psm, Animator a, AnimationClip barkAnimation)
    {
        playerStateMachine = psm;
        anim = a;
        Barkanim = barkAnimation;
    }
    public void TriggerBark()
    {
        playerStateMachine.StartCoroutine(PlayBarkAnim());
    }
    
    IEnumerator PlayBarkAnim()
    {
        anim.Play(Barkanim.name);
        yield return new WaitForSeconds(Barkanim.length);
        playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
    }
}
