using UnityEngine;

public abstract class PlayerStateBase : State
{
    protected PlayerStateMachine PlayerStateMachine;
    
    public PlayerStateBase(PlayerStateMachine playerStateMachine)
    {
        PlayerStateMachine = playerStateMachine;
    }

    protected void Move(Vector3 motion, float DeltaTime)
    {
        PlayerStateMachine.Controller.Move((motion + PlayerStateMachine.ForceReceiver.Movement) * DeltaTime);
    }
}