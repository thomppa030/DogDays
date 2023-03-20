using System;
using UnityEngine;

public class AudioTrigger : InLevelTrigger
{
    [Tooltip("If true, the audio clip will be played only once. If false, the audio clip will be played every time the player enters the trigger.")]
    
    [field: SerializeField] private AudioClip SoundClip { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        AudioUtilities.PlaySound(SoundClip);
        
        if (isOneShot)
        {
            Destroy(this, SoundClip.length);
        }
    }
}