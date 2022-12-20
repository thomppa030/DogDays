using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class DogDaysInputSystem : MonoBehaviour, IPlayerActions
    {
        [Header("Character Input Values")]
        public Vector2 moveInput;
        public Vector2 lookInput;
        
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }
        
        public void MoveInput(Vector2 newMoveDirection)
        {
            moveInput = newMoveDirection;
        }
        
        public void LookInput(Vector2 newLookDirection)
        {
            lookInput = newLookDirection;
        }
    }
}
