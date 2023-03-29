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
        
        //Acceleration here for Animation
        PlayerStateMachine.Animator.SetFloat(SpeedF, PlayerStateMachine.InputReader.MovementValue.magnitude, 0.1f, deltaTime);
        
        FaceMovementDirection(_movement, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        PlayerStateMachine.transform.rotation = Quaternion.Lerp(PlayerStateMachine.transform.rotation,
            Quaternion.LookRotation(movement), deltaTime * PlayerStateMachine.RotationDampening);
    }

    public override void OnStateExit()
    {
        _movement = Vector3.zero;
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
            if (dT.playOnTrigger == false)
            {
                InteractionManager.Instance.CurrentInteraction = dT.ActiveInteraction;
                dT.TriggerDialogue();
            }
        }
        else if (hit.collider.gameObject.TryGetComponent<WindowTrigger>(out WindowTrigger wt))
        {
            wt.Trigger();
        }
    }
    
    Vector3 CalculateRotation(float horizontal, float vertical)
    {
        Vector3 forward = PlayerStateMachine.MainCameraTransform.TransformDirection(Vector3.forward);
        
        forward.y = 0;
        forward = forward.normalized;
        
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;
        targetDirection = forward * vertical + right * horizontal;
        
        return targetDirection;
    }
}