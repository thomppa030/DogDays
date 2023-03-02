using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "new DialogueWaitingTime Asset", order = 2)]
public class DialogueWaitingTime : ScriptableObject
{
    [Header("Waiting Time")] [SerializeField]
    public float[] waitTime;
}