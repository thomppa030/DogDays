using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
        public State CurrentState { get; private set; }

        private void Update()
        {
                CurrentState?.Tick(Time.deltaTime);
        }
        
        public void SwitchState(State newState)
        {
                CurrentState?.OnStateExit();
                CurrentState = newState;
                CurrentState?.OnStateEnter();
        }
}