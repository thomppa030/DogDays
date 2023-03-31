using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "new CameraFocalPointData Asset", order = 3)]
public class CameraFocalPointData : ScriptableObject
{
    [Header("Camera Focal Points")] [SerializeField]
    public Transform[] cameraFocalPoints;
}
