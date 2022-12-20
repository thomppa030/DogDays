using UnityEngine.InputSystem;

namespace Interfaces
{
    public interface IPlayerActions
    {
        void OnMove(InputValue value);
        void OnLook(InputValue value);
    }
}