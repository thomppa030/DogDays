using System.Collections;
using UnityEngine;

public class CameraFocusState : CameraStateBase
{
    private Transform _focusPoint;
    private Transform _newCameraPosition;

    private Transform _cachedCameraPosition;

    public CameraFocusState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {
    }

    public CameraFocusState(CameraStateMachine cameraStateMachine, Transform newCameraPosition, Transform focusPoint) :
        base(cameraStateMachine)
    {
        _focusPoint = focusPoint;
        _newCameraPosition = newCameraPosition;
    }

    public override void OnStateEnter()
    {
        _cachedCameraPosition = CameraStateMachine.camera.transform;
        
        InteractionManager.Instance.OnResetCameraFocus += ResetFocus;
    }

    public override void Tick(float deltaTime)
    {
        Focus();
    }

    public override void OnStateExit()
    {
    }
    
    private void ResetFocus()
    {
        _focusPoint = null;
        _newCameraPosition = null;
        
        //Check if the distance between the camera and the cached position is greater than 0.1f
        if (Vector3.Distance(CameraStateMachine.camera.transform.position, _cachedCameraPosition.position) > 0.1f)
        {
            //Reset camera position
            CameraStateMachine.camera.transform.position = Vector3.Lerp(CameraStateMachine.camera.transform.position,
                _cachedCameraPosition.position, Time.deltaTime);
        }
        else
        {
            //Switch back to freelook state
            CameraStateMachine.SwitchState(new CameraFreelookState(CameraStateMachine));
        }
    }

    void Focus()
    {
        if (_focusPoint == null)
        {
            //Reset camera position
            CameraStateMachine.camera.transform.position = Vector3.Lerp(CameraStateMachine.camera.transform.position,
                _cachedCameraPosition.position, Time.deltaTime);
        }
        else
        {
            CameraStateMachine.camera.transform.LookAt(_focusPoint);
            CameraStateMachine.camera.transform.position = Vector3.Lerp(CameraStateMachine.camera.transform.position,
                _newCameraPosition.position, Time.deltaTime);
        }
    }
}