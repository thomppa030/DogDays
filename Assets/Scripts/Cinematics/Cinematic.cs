using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cinematics
{
    public class Cinematic : MonoBehaviour
    {
        [field: SerializeField] private PlayableDirector Director { get; set; }
        [field: SerializeField] private GameObject ControlPanel { get; set; }
        
        [field: SerializeField] public DialogueTrigger DialogueTrigger { get; set; }

        private void Awake()
        {
            Director.played += Director_Played;
            Director.stopped += Director_Stopped;
        }
        
        private void Director_Played(PlayableDirector obj)
        {
            ControlPanel.SetActive(false);
        }
        
        private void Director_Stopped(PlayableDirector obj)
        {
            ControlPanel.SetActive(true);
        }

        public void StartTimeLine()
        {
            Director.Play();
        }
    }
}
