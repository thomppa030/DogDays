using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [Header("Character Movement Variables")]
    [Tooltip("The maximum speed at which the character moves")]
    public float moveSpeed = 2.0f;
    public float speedChangeRate = 10.0f;
    
    [Range(0, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private DogDaysInputSystem _inputSystem;
    
    private float _maxSpeed;
    private float _speed;
    private float _targetRotation = 0.0f;
    private GameObject _mainCamera;
    private float _rotationVelocity;
    private float _verticalVelocity;


    private void Awake()
    {
        if (!_mainCamera)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");   
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inputSystem = GetComponent<DogDaysInputSystem>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        var targetSpeed = moveSpeed;
        if (_inputSystem.move == Vector2.zero)
        {
            return;
        }

        var velocity = _controller.velocity;
        var currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

        var speedOffset = 0.1f;
        var inputMagnitude = _inputSystem.move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed*inputMagnitude, Time.deltaTime * speedChangeRate);
            
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        
        var moveDirection = new Vector3(_inputSystem.move.x, 0.0f, _inputSystem.move.y).normalized;

        if (_inputSystem.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
            
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        
        var TargetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        
        _controller.Move(TargetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }
}