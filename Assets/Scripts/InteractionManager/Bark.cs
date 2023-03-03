using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private Animator anim;
    [field: SerializeField] private AnimationClip Barkanim { get; set; }
    private void Start()
    {
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }
    public void TriggerBark()
    {
        if (Barkanim != null)
        {
            anim.Play(Barkanim.name);
            //check if the animation is playing
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(Barkanim.name))
            {
                //check if the animation is playing
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    //animation is done playing
                    playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
                }
            }
        }
        else
        {
            Debug.Log("No bark animation found!");
        }
    }
}
