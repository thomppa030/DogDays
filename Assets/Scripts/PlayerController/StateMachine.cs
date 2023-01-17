using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
        private State _currentState;

        private void Update()
        {
                _currentState?.Tick(Time.deltaTime);
        }
        
        public void SwitchState(State newState)
        {
                _currentState?.OnStateExit();
                _currentState = newState;
                _currentState?.OnStateEnter();
        }
}