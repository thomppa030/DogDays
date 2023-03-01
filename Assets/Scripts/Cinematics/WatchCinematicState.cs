namespace Cinematics
{
    public class WatchCinematicState : PlayerStateBase
    {
        /*
         * Possible Behavior of the Player when watching a cinematic should be defined here.
         */
        public override void OnStateEnter()
        {
        }

        private void SkipCinematic()
        {
            
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void OnStateExit()
        {
        }

        public WatchCinematicState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
    }
}
