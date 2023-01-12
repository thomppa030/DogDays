

public abstract class PlayerStateBase : State
{
    protected PlayerStateMachine PlayerStateMachine;
    
    public PlayerStateBase(PlayerStateMachine playerStateMachine)
    {
        PlayerStateMachine = playerStateMachine;
    }
}