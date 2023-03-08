using System;
using System.Collections;
using System.Collections.Generic;
using CameraShake;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    public enum ShakeType
    {
        Explosion,
        ShortShake3D,
        StrongShake3D,
        Rumble
    };
    
    public ShakeType shakeType;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (shakeType)
            {
                case ShakeType.Explosion:
                    Debug.Log("Explosion");
                    break;
                case ShakeType.ShortShake3D:
                    CameraShaker.Presets.ShortShake3D(10,10,10);
                    break;
                case ShakeType.StrongShake3D:
                    CameraShaker.Presets.ShortShake3D(10,5,10);
                    break;
                case ShakeType.Rumble:
                    CameraShaker.Presets.ShortShake3D(5,7,10);
                    break;
            }
        }
    }
}
