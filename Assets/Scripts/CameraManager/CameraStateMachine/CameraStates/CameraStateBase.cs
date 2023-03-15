using UnityEngine;

public abstract class CameraStateBase : State
{
    protected CameraStateMachine CameraStateMachine;
    
    public CameraStateBase(CameraStateMachine cameraStateMachine)
    {
        CameraStateMachine = cameraStateMachine;
    }
}