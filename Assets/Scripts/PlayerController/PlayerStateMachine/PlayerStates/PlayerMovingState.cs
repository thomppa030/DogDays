﻿using UnityEngine;

public class PlayerMovingState : PlayerStateBase
{
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");

    public PlayerMovingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void OnStateEnter()
    {
        Debug.Log("Enter Player Moving State");
        
        PlayerStateMachine.InputReader.InteractEvent += Interact;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        
        Move(movement * PlayerStateMachine.MovementSpeed, deltaTime);

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
            PlayerStateMachine.Bark.TriggerBark();
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