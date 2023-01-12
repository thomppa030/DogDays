using UnityEngine;

public class PlayerTestState : PlayerStateBase
{
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");

    private const float AnimationDampTime = 0.1f;

    public PlayerTestState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void OnStateEnter()
    {
        PlayerStateMachine.InputReader.JumpEvent += Jump;
        Debug.Log("Enter Player Test State");
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        PlayerStateMachine.Controller.Move(movement * (PlayerStateMachine.MovementSpeed * deltaTime));

        if (PlayerStateMachine.InputReader.MovementValue == Vector2.zero)
        {
            PlayerStateMachine.Animator.SetFloat(SpeedF,0, 0.1f, deltaTime);
            return;
        }
        
        PlayerStateMachine.Animator.SetFloat(SpeedF,1, 0.1f, deltaTime);
        
        FaceMovementDirection(movement, deltaTime);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        PlayerStateMachine.transform.rotation = Quaternion.Lerp(PlayerStateMachine.transform.rotation,
            Quaternion.LookRotation(movement), deltaTime * PlayerStateMachine.RotationDampening);
    }


    private void Jump()
    {
            
    }

    public override void OnStateExit()
    {
        PlayerStateMachine.InputReader.JumpEvent -= Jump;
        Debug.Log("Exit Player Test State");
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