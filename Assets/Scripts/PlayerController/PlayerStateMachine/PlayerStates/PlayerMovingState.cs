using UnityEngine;

public class PlayerMovingState : PlayerStateBase
{
    Vector3 _movement;
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");

    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void OnStateEnter()
    {
        _movement = Vector3.zero;
        
        Debug.Log("Enter Player Moving State");
        
        PlayerStateMachine.InputReader.InteractEvent += Interact;
    }

    public override void Tick(float deltaTime)
    {
        _movement = CalculateRotation(PlayerStateMachine.InputReader.MovementValue.x,
            PlayerStateMachine.InputReader.MovementValue.y);
        
        Move(_movement * PlayerStateMachine.MovementSpeed, deltaTime);

        if (PlayerStateMachine.InputReader.MovementValue == Vector2.zero)
        {
            PlayerStateMachine.Animator.SetFloat(SpeedF,0, 0.1f, deltaTime);
            return;
        }
        
        PlayerStateMachine.Animator.SetFloat(SpeedF,1, 0.1f, deltaTime);
        
        FaceMovementDirection(_movement, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        PlayerStateMachine.transform.rotation = Quaternion.Lerp(PlayerStateMachine.transform.rotation,
            Quaternion.LookRotation(movement), deltaTime * PlayerStateMachine.RotationDampening);
    }

    public override void OnStateExit()
    {
    }

    void Interact()
    {
        bool interActionIsInRange = PlayerStateMachine.InteractionCheckRayCast.InteractionInRange();
        
        if (interActionIsInRange)
        {
            StartInteraction();
        }
        else
        {
            PlayerStateMachine.SwitchState(new PlayerBarkingstate(PlayerStateMachine));
        }
    }
    
    private void StartInteraction()
    {
        RaycastHit hit;
        hit = PlayerStateMachine.InteractionCheckRayCast.hit;
        
        if (PlayerStateMachine != null)
            PlayerStateMachine.SwitchState(new PlayerInteractState(PlayerStateMachine));

        Debug.Log("Interacting with: " + hit.collider.gameObject.name);
        if (hit.collider.gameObject.TryGetComponent<InteractionTrigger>(out InteractionTrigger dT))
        {
            GameState.Instance.currentlyActiveInteraction = dT;
            dT.TriggerDialogue();
        }
        else
        {
            Debug.LogError("No Dialogue attached to: " + hit.collider.gameObject.name);
        }
    }
    
    Vector3 CalculateRotation(float horizontal, float vertical)
    {
        Vector3 forward = PlayerStateMachine.MainCameraTransform.forward;
        
        forward.y = 0;
        forward = forward.normalized;
        
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * vertical + right * horizontal;
        
        return targetDirection;
    }
    
    public Vector3 CalculateMovement()
    {
        Vector3 CameraForward = PlayerStateMachine.MainCameraTransform.forward;
        Vector3 CameraRight = PlayerStateMachine.MainCameraTransform.right;
        
        CameraForward.y = 0;
        CameraRight.y = 0;
        
        CameraForward.Normalize();
        CameraRight.Normalize();
     
        return CameraForward * PlayerStateMachine.InputReader.MovementValue.y + CameraRight * PlayerStateMachine.InputReader.MovementValue.x;
    }
}