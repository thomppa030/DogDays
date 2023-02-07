using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [field: SerializeField] private AudioSource AudioSource { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Play {AudioSource.clip.name} audio clip");
            AudioSource.Play();
            Destroy(this);
        }
    }
}
