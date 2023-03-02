using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueAnimations Asset", order = 4)]
public class DialogueAnimations : ScriptableObject
{
    [Header("Animations")] [SerializeField]
    public AnimationClip[] characterAnim;
}
