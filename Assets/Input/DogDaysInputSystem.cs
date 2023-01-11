using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class DogDaysInputSystem : MonoBehaviour
{
    [Header("Character Input Fields")]
    public Vector2 move;
    public Vector2 look;
    
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }
    
    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }
    
    public void MoveInput(Vector2 value)
    {
        move = value;
    }
    
    public void LookInput(Vector2 value)
    {
        look = value;
    }
}