using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : InLevelTrigger
{
    public bool IsOneShot { get; set; }
    
    private Animator _anim;
    [field: SerializeField] public AnimationClip AnimationClip { get; set; }

    private void OnEnable()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Trigger()
    {
        _anim.Play(AnimationClip.name);
        
        if (IsOneShot)
        {
            Destroy(this, AnimationClip.length);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }
}
