using System;
using CameraShake;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 pivotOffset = new Vector3(0.0f,1.7f,0.0f);
    public Vector3 cameraOffset = new Vector3(0.0f,0.0f,-3.0f);
    public float smooth = 10f;
    public float horizontalAimingSpeed = 2.0f;
    public float verticalAimingSpeed = 2.0f;
    public float maxVerticalAngle = 30.0f;
    public float minVerticalAngle = -60.0f;
    
    public Transform currentCameraTransform;
    
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
    
    [SerializeField] private PlayerStateMachine playerStateMachine;
    
    public float GetH { get { return angleH; } }

    private void Awake()
    {
        cameraTransform = transform;
        cameraTransform.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * cameraOffset;
        cameraTransform.rotation = Quaternion.identity;
        
        smoothPivotOffset = pivotOffset;
        smoothCameraOffset = cameraOffset;
        defaultFOV = cameraTransform.GetComponent<Camera>().fieldOfView;
        angleH = player.eulerAngles.y;
        
        currentCameraTransform = cameraTransform;
        
        ResetTargetOffsets();
        ResetFOV();
        ResetMaxVerticalAngle();
        
        if (cameraOffset.y > 0)
        {
            Debug.LogWarning("Vertical Cam Offset (Y) will be ignored during collisions!\n" +
                             "It is recommended to set all vertical offset in Pivot Offset.");
        }
    }

	void Update()
	{
		if (!CameraShaker.IsShaking)
		{
			// Get mouse movement to orbit the camera.
			// Mouse:
			angleH += playerStateMachine.InputReader.LookValue.x * horizontalAimingSpeed;
			angleV += playerStateMachine.InputReader.LookValue.y * verticalAimingSpeed;
			// Joystick:
			angleH += playerStateMachine.InputReader.MovementValue.x * 60 * horizontalAimingSpeed * Time.deltaTime;
			angleV += playerStateMachine.InputReader.MovementValue.y * 60 * verticalAimingSpeed * Time.deltaTime;

			// Set vertical movement limit.
			angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

			// Set camera orientation.
			Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
			Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
			cameraTransform.rotation = aimRotation;

			// Set FOV.
			cameraTransform.GetComponent<Camera>().fieldOfView =
				Mathf.Lerp(cameraTransform.GetComponent<Camera>().fieldOfView, targetFOV, Time.deltaTime);

			// Test for collision with the environment based on current camera position.
			Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
			Vector3 noCollisionOffset = targetCameraOffset;
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
				isCustomOffset && noCollisionOffset.sqrMagnitude < targetCameraOffset.sqrMagnitude;

			// Repostition the camera.
			smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, customOffsetCollision ? pivotOffset : targetPivotOffset,
				smooth * Time.deltaTime);
			smoothCameraOffset = Vector3.Lerp(smoothCameraOffset,
				customOffsetCollision ? Vector3.zero : noCollisionOffset, smooth * Time.deltaTime);

			// if the Camera is shaking, we don't want to move it around
			cameraTransform.position =
				player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCameraOffset;
		}
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
		targetPivotOffset = pivotOffset;
		targetCameraOffset = cameraOffset;
		isCustomOffset = false;
	}

	// Reset the camera vertical offset.
	public void ResetYCamOffset()
	{
		targetCameraOffset.y = cameraOffset.y;
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
		this.targetMaxVerticalAngle = maxVerticalAngle;
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
		Vector3 target = player.position + pivotOffset;
		Vector3 direction = target - checkPos;
		// If a raycast from the check position to the player hits something...
		if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude))
		{
			// ... if it is not the player...
			if(hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
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
		Vector3 origin = player.position + pivotOffset;
		Vector3 direction = checkPos - origin;
		if (Physics.SphereCast(origin, 0.2f, direction, out RaycastHit hit, direction.magnitude))
		{
			if(hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
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
