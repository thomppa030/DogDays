using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTrigger : MonoBehaviour
{
    [SerializeField]
    private CameraStateMachine cameraStateMachine;
    
    [SerializeField]
    private Transform cameraFocusPoint;
    
    [SerializeField]
    private Transform cameraFocusPosition;
    
    public void Trigger()
    {
        cameraStateMachine.SwitchState(new CameraWindowStationState(cameraStateMachine, cameraFocusPosition, cameraFocusPoint));
    }
}
