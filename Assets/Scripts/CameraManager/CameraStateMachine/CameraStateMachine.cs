using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateMachine : StateMachine
{
    public InputReader inputReader;
    
    public GameObject camera;

	[Header("Follow Camera Settings")]
    public Transform player;
    public Vector3 pivotOffset = new Vector3(0.0f,1.7f,0.0f);
    public Vector3 cameraOffset = new Vector3(0.0f,0.0f,-3.0f);
    public float smooth = 10f;
    public float horizontalAimingSpeed = 2.0f;
    public float verticalAimingSpeed = 2.0f;
    public float maxVerticalAngle = 30.0f;
    public float minVerticalAngle = -60.0f;
    
    [SerializeField] private PlayerStateMachine playerStateMachine;
    
    
    private void Awake()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    
    private void Start()
    {
        SwitchState(new CameraFreelookState(this));
    }
}
