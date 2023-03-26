using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerStateBase
{
    private static readonly int SpeedF = Animator.StringToHash("Speed_f");
    
    public PlayerInteractState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void OnStateEnter()
    {
        PlayerStateMachine.Animator.SetFloat(SpeedF, 0.0f, 0.0f, 0.0f);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void OnStateExit()
    {
    }
}
