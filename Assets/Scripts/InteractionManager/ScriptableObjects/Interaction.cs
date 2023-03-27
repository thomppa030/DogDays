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
        DisableInfoDisplay = 12,
        DisablePlayerMovement = 18,
        DisableProfileImage = 14,
        DisableTextDisplay  = 2,
        EnablePlayerMovement = 19,
        EnableTextDisplay = 1,
        EndDialogue = 8,
        FadeIn = 4,
        FadeOut = 5,
        LoadNextScene = 9,
        NextSentence = 0,
        NextSentenceWithWait = 15,
        PlayCharAnim = 10,
        PlayCharAnimWithWait = 17,
        PlaySfx = 7,
        PlaySfxImmediate = 16,
        ResetCameraFocus = 21,
        SetProfileImage = 13,
        ShakeCamera = 6,
        ShowInfoDisplay = 11,
        SwitchCameraFocus = 20,
        Wait = 3,
    }
}