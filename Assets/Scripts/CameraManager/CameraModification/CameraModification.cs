using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This class is used to modify the camera in real time,
 * to Lerp the Camera to focus on a specific Object
 */

public class CameraModification : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public CameraModification(Transform target, float smoothSpeed, Vector3 offset)
    {
        this.target = target;
        this.smoothSpeed = smoothSpeed;
        this.offset = offset;
    }
    
    public CameraModification(Transform target, Vector3 offset)
    {
        this.target = target;
        this.offset = offset;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
