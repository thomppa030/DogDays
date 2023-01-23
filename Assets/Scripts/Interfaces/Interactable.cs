
namespace Interfaces
{
    public interface IInteractable
    {
        void OnInteract();
        void OnInteractCancel();
        
        void Lock();
        void Unlock();
    }
}