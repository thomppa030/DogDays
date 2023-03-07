using System;
using System.Collections;
using System.Collections.Generic;
using CameraShake;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Shake");
            CameraShaker.Presets.ShortShake3D(10,5,10);
        }
    }
}
