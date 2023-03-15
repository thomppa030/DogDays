using UnityEngine;

public class CameraFocusState : CameraStateBase
{
    private CameraModification _cameraModification;
    
    public CameraFocusState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {
        
    }
    public CameraFocusState(CameraStateMachine cameraStateMachine, CameraModification cameraModification) : base(cameraStateMachine)
    {
        _cameraModification = cameraModification;
    }
    
    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
}