using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Interaction : ScriptableObject
{
    [field: SerializeField] public bool startOnAwake { get; set; }
    [field: SerializeField] public Action[] Actions { get; set; }

    public Dialogue AssignedDialogue;
    
    //TODO: Split into serialized Objects
    [field: SerializeField] public float[] waitTime { get; set; }
    [field: SerializeField] public AnimationClip[] characterAnim { get; set; }
    [field: SerializeField] public Sprite[] profileImages { get; set; }

    public enum Action
    {
        nextSentence,
        enableTextDisplay,
        disableTextDisplay,
        wait,
        fadeIn,
        fadeOut,
        shakeCamera,
        playSFX,
        endDialogue,
        loadNextScene,
        playCharAnim,
        showInfoDisplay,
        disableInfoDisplay,
        setProfileImage,
        disableProfileImage,
        nextSentenceWithWait,
        playSFXImmediate,
        playCharAnimWithWait,
        playNextUIAnimation,
        fadeOutUIAnimation,
        disablePlayerMovement,
        enablePlayerMovement,
        TriggerVideoAnimationDay01,
        TriggerVideoAnimationDay02,
        HideVideoPanel01Day01,
        HideVideoPanelDay02,
        ResetAnimID,
        HideVideoPanel02Day01,
    }
    
    [Space] [Header("SFX")] [SerializeField]
    private AudioClip[] Audioclips;
    
    public AudioClip GetAudioClip(int id)
    {
        return Audioclips[id];
    }
}
