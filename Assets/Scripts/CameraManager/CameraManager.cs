using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [field: SerializeField] public CinemachineFreeLook FreeLookCamera { get; private set; }
    void Start()
    {
            InteractionManager.Instance.OnSwitchCameraFocus += LerpCameraToFocusPoint;
    }

    public void LerpCameraToFocusPoint()
    {
        Debug.Log("Camera should lerp to new focus point now, lol!");
    }
}