using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "NewText", order = 1)]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] sentences;
}
