using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioTrigger : InLevelTrigger
{
    
    [SerializeField]
    private AudioSource soundSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Trigger();
        }
    }

    public override void Trigger()
    {
        soundSource.Play();
        
        if (isOneShot)
        {
            Destroy(this, soundSource.clip.length);
        }
    }
}