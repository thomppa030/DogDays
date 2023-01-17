using System.Collections;

public abstract class State
{
    public abstract void OnStateEnter();
    public abstract void Tick(float deltaTime);
    public abstract void OnStateExit();
}