using UnityEngine;
using UnityEngine.Timeline;

namespace Cinematics
{
    public class Cinematic : MonoBehaviour
    {
        [field: SerializeField] public TimelineAsset CinematicTimeline { get; private set; }
        [field: SerializeField] public float CinematicDuration { get; private set; }
        
        /*
         * Subtitle Object maybe usable from Dialogue System?
         * not sure if it's a good idea to use it here
         */
    
        // Event that triggers when the cinematic is finished
        public delegate void CinematicFinished();
        public static event CinematicFinished OnCinematicFinished;

        public void Play()
        {
            // Make the Timeline play somehow
            // ...
            // ...

            // Trigger the event when the Timeline is finished
            if (CinematicTimeline.duration >= CinematicDuration)
            {
                OnCinematicFinished?.Invoke();
            }
        }
    }
}
