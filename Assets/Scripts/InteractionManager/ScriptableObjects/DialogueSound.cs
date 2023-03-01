using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueWaitingTime Asset", order = 2)]
public class DialogueWaitingTime : ScriptableObject
{
    [Header("Waiting Time")] [SerializeField]
    public float[] waitTime;
}

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueSounds Asset", order = 3)]
public class DialogueSound : ScriptableObject
{
    [Header("SFX")] [SerializeField]
    public AudioClip[] audioclips;
}

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueAnimations Asset", order = 4)]
public class DialogueAnimations : ScriptableObject
{
    [Header("Animations")] [SerializeField]
    public AnimationClip[] characterAnim;
}

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueDogFaces Asset", order = 5)]
public class DialogueDogFaces : ScriptableObject
{
    [Header("Dog Faces")] [SerializeField]
    public Sprite[] dogFaces;
}