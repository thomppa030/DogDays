using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "new DialogueDogFaces Asset", order = 5)]
public class DialogueDogFaces : ScriptableObject
{
    [Header("Dog Faces")] [SerializeField]
    public Sprite[] dogFaces;
}