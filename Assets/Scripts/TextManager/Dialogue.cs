using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "NewText", order = 1)]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] ger_sentences;

    [TextArea(3, 10)]
    public string[] eng_sentences;
}
