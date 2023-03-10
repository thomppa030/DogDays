using UnityEngine;

public class CameraFreelookState : CameraStateBase
{
    
    private float angleH = 0;
    private float angleV = 0;
    private Transform cameraTransform;
    private Vector3 smoothPivotOffset;
    private Vector3 smoothCameraOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCameraOffset;
    private float defaultFOV;
    private float targetFOV;
    private float targetMaxVerticalAngle;
    private bool isCustomOffset;
    
    public float GetH { get { return angleH; } }
    
    public CameraFreelookState(CameraStateMachine cameraStateMachine) : base(cameraStateMachine)
    {}
    
    public override void OnStateEnter()
    {
        cameraTransform = CameraStateMachine.transform;
        cameraTransform.position = CameraStateMachine.player.position +
                                   Quaternion.identity * CameraStateMachine.pivotOffset +
                                   Quaternion.identity * CameraStateMachine.cameraOffset;
        cameraTransform.rotation = Quaternion.identity;
        
        smoothPivotOffset = CameraStateMachine.pivotOffset;
        smoothCameraOffset = CameraStateMachine.cameraOffset;
        defaultFOV = cameraTransform.GetComponent<Camera>().fieldOfView;
        angleH = CameraStateMachine.player.eulerAngles.y;
        
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
        
    }

    public override void OnStateExit()
    {
        throw new System.NotImplementedException();
    }
    
	// Set camera offsets to custom values.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCameraOffset = newCamOffset;
		isCustomOffset = true;
	}

	// Reset camera offsets to default values.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = CameraStateMachine.pivotOffset;
		targetCameraOffset = CameraStateMachine.cameraOffset;
		isCustomOffset = false;
	}

	// Reset the camera vertical offset.
	public void ResetYCamOffset()
	{
		targetCameraOffset.y = CameraStateMachine.cameraOffset.y;
	}

	// Set camera vertical offset.
	public void SetYCamOffset(float y)
	{
		targetCameraOffset.y = y;
	}

	// Set camera horizontal offset.
	public void SetXCamOffset(float x)
	{
		targetCameraOffset.x = x;
	}

	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle.
	public void SetMaxVerticalAngle(float angle)
	{
		this.targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value.
	public void ResetMaxVerticalAngle()
	{
		this.targetMaxVerticalAngle = CameraStateMachine.maxVerticalAngle;
	}

	// Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
	bool DoubleViewingPosCheck(Vector3 checkPos)
	{
		return ViewingPosCheck (checkPos) && ReverseViewingPosCheck (checkPos);
	}

	// Check for collision from camera to player.
	bool ViewingPosCheck (Vector3 checkPos)
	{
		// Cast target and direction.
		Vector3 target = CameraStateMachine.player.position + CameraStateMachine.pivotOffset;
		Vector3 direction = target - checkPos;
		// If a raycast from the check position to the player hits something...
		if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude))
		{
			// ... if it is not the player...
			if(hit.transform != CameraStateMachine.player && !hit.transform.GetComponent<Collider>().isTrigger)
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
			if(hit.transform != CameraStateMachine.player && hit.transform != CameraStateMachine.transform && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				return false;
			}
		}
		return true;
	}

	// Get camera magnitude.
	public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}