using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//YAGNI
[CreateAssetMenu(fileName = "Data", menuName = "new CameraFocalPointData Asset", order = 3)]
public class CameraFocalPointData : ScriptableObject
{
    [Header("Camera Focal Points")] [SerializeField]
    public Transform[] cameraFocalPoints;
}
