using UnityEngine;

public class CameraFreelookState : CameraStateBase
{
    private float _angleH = 0;
    private float _angleV = 0;
    private Transform _cameraTransform;
    private Vector3 _smoothPivotOffset;
    private Vector3 _smoothCameraOffset;
    private Vector3 _targetPivotOffset;
    private Vector3 _targetCameraOffset;
    private float _defaultFOV;
    private float _targetFOV;
    private float _targetMaxVerticalAngle;
    private bool _isCustomOffset;

    public float GetH
    {
        get { return _angleH; }
    }

    public CameraFreelookState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {
    }

    public override void OnStateEnter()
    {
        _cameraTransform = CameraStateMachine.transform;
        _cameraTransform.position = CameraStateMachine.player.position +
                                    Quaternion.identity * CameraStateMachine.pivotOffset +
                                    Quaternion.identity * CameraStateMachine.cameraOffset;
        _cameraTransform.rotation = Quaternion.identity;

        _smoothPivotOffset = CameraStateMachine.pivotOffset;
        _smoothCameraOffset = CameraStateMachine.cameraOffset;
        _defaultFOV = _cameraTransform.GetComponent<Camera>().fieldOfView;
        _angleH = CameraStateMachine.player.eulerAngles.y;

        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();

        if (CameraStateMachine.cameraOffset.y > 0)
        {
            Debug.LogWarning("Vertical Cam Offset (Y) will be ignored during collisions!\n" +
                             "It is recommended to set all vertical offset in Pivot Offset.");
        }

        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime)
    {
        // Get mouse movement to orbit the camera.
        // Mouse:
        _angleH += Mathf.Clamp(CameraStateMachine.inputReader.LookValue.x, -1, 1) *
                   CameraStateMachine.horizontalAimingSpeed;
        _angleV += Mathf.Clamp(CameraStateMachine.inputReader.LookValue.y, -1, 1) *
                   CameraStateMachine.verticalAimingSpeed;
        // Joystick:
        // angleH += playerStateMachine.InputReader.MovementValue.x * 60 * horizontalAimingSpeed * Time.deltaTime;
        // angleV += playerStateMachine.InputReader.MovementValue.y * 60 * verticalAimingSpeed * Time.deltaTime;

        // Set vertical movement limit.
        _angleV = Mathf.Clamp(_angleV, CameraStateMachine.minVerticalAngle, _targetMaxVerticalAngle);

        // Set camera orientation.
        Quaternion camYRotation = Quaternion.Euler(0, _angleH, 0);
        Quaternion aimRotation = Quaternion.Euler(-_angleV, _angleH, 0);
        _cameraTransform.rotation = aimRotation;

        // Set FOV.
        _cameraTransform.GetComponent<Camera>().fieldOfView =
            Mathf.Lerp(_cameraTransform.GetComponent<Camera>().fieldOfView, _targetFOV, Time.deltaTime);

        // Test for collision with the environment based on current camera position.
        Vector3 baseTempPosition = CameraStateMachine.player.position + camYRotation * _targetPivotOffset;
        Vector3 noCollisionOffset = _targetCameraOffset;
        while (noCollisionOffset.magnitude >= 0.2f)
        {
            if (DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset))
                break;
            noCollisionOffset -= noCollisionOffset.normalized * 0.2f;
        }

        if (noCollisionOffset.magnitude < 0.2f)
            noCollisionOffset = Vector3.zero;

        // No intermediate position for custom offsets, go to 1st person.
        bool customOffsetCollision =
            _isCustomOffset && noCollisionOffset.sqrMagnitude < _targetCameraOffset.sqrMagnitude;

        // Repostition the camera.
        _smoothPivotOffset = Vector3.Lerp(_smoothPivotOffset,
            customOffsetCollision ? CameraStateMachine.pivotOffset : _targetPivotOffset,
            CameraStateMachine.smooth * Time.deltaTime);
        _smoothCameraOffset = Vector3.Lerp(_smoothCameraOffset,
            customOffsetCollision ? Vector3.zero : noCollisionOffset, CameraStateMachine.smooth * Time.deltaTime);

        _cameraTransform.position =
            CameraStateMachine.player.position + camYRotation * _smoothPivotOffset + aimRotation * _smoothCameraOffset;
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    // Set camera offsets to custom values.
    public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        _targetPivotOffset = newPivotOffset;
        _targetCameraOffset = newCamOffset;
        _isCustomOffset = true;
    }

    // Reset camera offsets to default values.
    public void ResetTargetOffsets()
    {
        _targetPivotOffset = CameraStateMachine.pivotOffset;
        _targetCameraOffset = CameraStateMachine.cameraOffset;
        _isCustomOffset = false;
    }

    // Reset the camera vertical offset.
    public void ResetYCamOffset()
    {
        _targetCameraOffset.y = CameraStateMachine.cameraOffset.y;
    }

    // Set camera vertical offset.
    public void SetYCamOffset(float y)
    {
        _targetCameraOffset.y = y;
    }

    // Set camera horizontal offset.
    public void SetXCamOffset(float x)
    {
        _targetCameraOffset.x = x;
    }

    // Set custom Field of View.
    public void SetFOV(float customFOV)
    {
        this._targetFOV = customFOV;
    }

    // Reset Field of View to default value.
    public void ResetFOV()
    {
        this._targetFOV = _defaultFOV;
    }

    // Set max vertical camera rotation angle.
    public void SetMaxVerticalAngle(float angle)
    {
        this._targetMaxVerticalAngle = angle;
    }

    // Reset max vertical camera rotation angle to default value.
    public void ResetMaxVerticalAngle()
    {
        this._targetMaxVerticalAngle = CameraStateMachine.maxVerticalAngle;
    }

    // Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
    bool DoubleViewingPosCheck(Vector3 checkPos)
    {
        return ViewingPosCheck(checkPos) && ReverseViewingPosCheck(checkPos);
    }

    // Check for collision from camera to player.
    bool ViewingPosCheck(Vector3 checkPos)
    {
        // Cast target and direction.
        Vector3 target = CameraStateMachine.player.position + CameraStateMachine.pivotOffset;
        Vector3 direction = target - checkPos;
        // If a raycast from the check position to the player hits something...
        if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            // ... if it is not the player...
            if (hit.transform != CameraStateMachine.player && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                // This position isn't appropriate.
                return false;
            }
        }

        // If we haven't hit anything or we've hit the player, this is an appropriate position.
        return true;
    }

    // Check for collision from player to camera.
    bool ReverseViewingPosCheck(Vector3 checkPos)
    {
        // Cast origin and direction.
        Vector3 origin = CameraStateMachine.player.position + CameraStateMachine.pivotOffset;
        Vector3 direction = checkPos - origin;
        if (Physics.SphereCast(origin, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            if (hit.transform != CameraStateMachine.player && hit.transform != CameraStateMachine.transform &&
                !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }

        return true;
    }

    // Get camera magnitude.
    public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
    {
        return Mathf.Abs((finalPivotOffset - _smoothPivotOffset).magnitude);
    }
}