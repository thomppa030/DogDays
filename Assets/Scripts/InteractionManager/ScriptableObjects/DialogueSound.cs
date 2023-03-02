using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Data", menuName = "new DialogueSounds Asset", order = 3)]
public class DialogueSound : ScriptableObject
{
    [Header("SFX")] [SerializeField]
    public AudioClip[] audioclips;
}

