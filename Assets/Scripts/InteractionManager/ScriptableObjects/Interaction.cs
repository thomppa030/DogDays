using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "new Interaction Asset", order = 1)]
public class Interaction : ScriptableObject
{
    [field: SerializeField] public bool StartOnAwake { get; set; }
    [field: SerializeField] public Action[] Actions { get; set; }

    public Dialogue assignedDialogue;
    public DialogueSound dialogueSounds;
    public DialogueWaitingTime dialogueWaitingTime;
    public DialogueAnimations dialogueAnimations;
    public DialogueDogFaces dialogueDogFaces;
    public CameraFocalPointData cameraFocalPoints;
    
    public enum Action
    {
        NextSentence,
        EnableTextDisplay,
        DisableTextDisplay,
        Wait,
        FadeIn,
        FadeOut,
        ShakeCamera,
        PlaySfx,
        EndDialogue,
        LoadNextScene,
        PlayCharAnim,
        ShowInfoDisplay,
        DisableInfoDisplay,
        SetProfileImage,
        DisableProfileImage,
        NextSentenceWithWait,
        PlaySfxImmediate,
        PlayCharAnimWithWait,
        DisablePlayerMovement,
        EnablePlayerMovement,
        SwitchCameraFocus,
        TriggerVideoAnimationDay01,
        TriggerVideoAnimationDay02,
        HideVideoPanel01Day01,
        HideVideoPanelDay02,
        ResetAnimID,
        HideVideoPanel02Day01,
    }
    public List<Interaction> interactionToEnable = new List<Interaction>();
    public List<Interaction> interactionToDisable = new List<Interaction>();
}
