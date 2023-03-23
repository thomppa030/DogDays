using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWindowStationState : CameraStateBase
{
    private float _angleY;
    private float _angleH;

    private Transform _cameraPos;
    private Camera _camera;
    
    public CameraWindowStationState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {
    }
    
    public CameraWindowStationState(CameraStateMachine cameraStateMachine, Transform cameraPos) : base(cameraStateMachine)
    {
        _cameraPos = cameraPos;
    }

    public override void OnStateEnter()
    {
        Cursor.visible = true;
        
        _camera = CameraStateMachine.camera.GetComponent<Camera>();
        
        _camera.orthographic = true;
    }

    public override void Tick(float deltaTime)
    {
        ClampCameraRotation();
    }

    public override void OnStateExit()
    {
        Cursor.visible = false;
        _camera.orthographic = false;
    }
    
    // May not be useful with orthographic camera
    void ClampCameraRotation()
    {
        _angleH += Mathf.Clamp(CameraStateMachine.inputReader.LookValue.x, -1, 1);
        _angleY += Mathf.Clamp(CameraStateMachine.inputReader.LookValue.y, -1, 1);
        _angleH = Mathf.Clamp(_angleH, -30, 30);
        _angleY = Mathf.Clamp(_angleY, -30, 30);
        
        var camYRotation = Quaternion.Euler(0, _angleH, 0);
        var camAimRotation = Quaternion.Euler(-_angleY, _angleH, 0);
        CameraStateMachine.transform.rotation = camYRotation;
    }
}
