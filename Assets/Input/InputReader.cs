using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

    public class InputReader : MonoBehaviour, DogDaysInput.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
        
        public event Action TriggerInteractionEvent;

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
        
        public void OnMove(InputAction.CallbackContext context)
        {
             MovementValue = context.ReadValue<Vector2>();
        }
        
        public void OnTriggerInteraction(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            TriggerInteractionEvent?.Invoke();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }
    }