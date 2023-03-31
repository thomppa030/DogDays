using UnityEngine;

public class CameraWindowStationState : CameraFocusState
{
    private float _angleY;
    private float _angleH;

    private Transform _cameraPos;
    private Camera _camera;
    
    PlayerStateMachine _playerStateMachine;
    
    public CameraWindowStationState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {
    }

    public CameraWindowStationState(CameraStateMachine cameraStateMachine, PlayerStateMachine playerStateMachine, Transform newCameraPosition, Transform focusPoint) : base(cameraStateMachine, newCameraPosition, focusPoint)
    {
        _focusPoint = focusPoint;
        _newCameraPosition = newCameraPosition;
        _playerStateMachine = playerStateMachine;
    }

    public override void OnStateEnter()
    {
        _playerStateMachine.InputReader.InteractEvent += () =>
        {
            CameraStateMachine.SwitchState(new CameraFreelookState(CameraStateMachine));
            _playerStateMachine.SwitchState(new PlayerMovingState(_playerStateMachine));
        };
        
        _camera = CameraStateMachine.camera.GetComponent<Camera>();
    }

    public override void Tick(float deltaTime)
    {
        Focus();
    }

    public override void OnStateExit()
    {
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
    
protected override void Focus()
    {
        //Check if the distance between the camera and the new position is greater than 0.1f
        if (Vector3.Distance(CameraStateMachine.camera.transform.position, _newCameraPosition.position) > 0.1f)
        {
            CameraStateMachine.camera.transform.LookAt(_focusPoint);
            //Move camera to new position
            CameraStateMachine.camera.transform.position = Vector3.Lerp(CameraStateMachine.camera.transform.position,
                _newCameraPosition.position, Time.deltaTime);
        }
    }
}
