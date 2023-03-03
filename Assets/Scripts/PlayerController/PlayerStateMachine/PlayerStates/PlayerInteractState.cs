using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerStateBase
{
    public PlayerInteractState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        
    }
    
    public override void OnStateEnter()
    {
        PlayerStateMachine.InputReader.SkipSentenceEvent += SkipSentence;
    }

    void SkipSentence()
    {
        InteractionManager.Instance.TriggerNextSentence();
    }
    
    public override void Tick(float deltaTime)
    {
    }

    public override void OnStateExit()
    {
    }
}
