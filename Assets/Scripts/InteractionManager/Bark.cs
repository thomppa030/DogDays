using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    Animator anim;
    [field: SerializeField] private AnimationClip Barkanim { get; set; }
    public void TriggerBark()
    {
        if (Barkanim != null)
        {
            anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
            anim.Play(Barkanim.name);
        }
        else
        {
            Debug.Log("No bark animation found!");
        }
    }
}
