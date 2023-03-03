using System;
using Cinemachine;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public Bark Bark { get; private set; }
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDampening { get; private set; }
    [field: SerializeField] public Transform MainCameraTransform { get; private set; }
    public InteractionCheckRayCast InteractionCheckRayCast { get; private set; }
    
    private GameObject _camera;

    private void Awake()
    {
        if (_camera == null)
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        MainCameraTransform = _camera.transform;
        
        InteractionCheckRayCast = GetComponent<InteractionCheckRayCast>();

        SwitchState(new PlayerMovingState(this));
    }
}