using System;
using UnityEngine;

namespace Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        [field: SerializeField] private PlayerStateMachine PlayerStateMachine { get; set; }
        [field: SerializeField] public Cinematic Cinematic { get; private set; }
        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider is the player
            if (other.CompareTag("Player"))
            {
                PlayerStateMachine.SwitchState(new WatchCinematicState());
                Cinematic.Play();
            }
        }
    }
}
