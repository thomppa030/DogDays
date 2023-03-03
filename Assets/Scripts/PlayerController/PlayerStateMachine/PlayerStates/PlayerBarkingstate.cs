using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarkingstate : PlayerStateBase
{
    public PlayerBarkingstate(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void OnStateEnter()
    {
        PlayerStateMachine.Bark.TriggerBark();
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void OnStateExit()
    {
    }
}
