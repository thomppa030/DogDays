using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioTrigger : InLevelTrigger
{
    
    [SerializeField]
    private Sound soundClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        soundClip.Play();
        
        if (isOneShot)
        {
            Destroy(this, soundClip.clip.length);
        }
    }
}