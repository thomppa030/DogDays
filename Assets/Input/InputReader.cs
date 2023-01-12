using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

    public class InputReader : MonoBehaviour, DogDaysInput.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
        
        public event Action JumpEvent; 

        private DogDaysInput _input;
        private void Start()
        {
            _input = new DogDaysInput();
            _input.Player.SetCallbacks(this);
            
            _input.Player.Enable();
        }

        private void OnDestroy()
        {
            _input.Player.Disable();
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            JumpEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
             MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }
    }