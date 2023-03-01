using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "NewText", order = 1)]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private string Title = "default Title";

    [Space] [Header("Sentences")] [TextArea(3, 10)]
    public List<string> ger_sentences = new List<string>();

    public List<string> ger_keywords = new List<string>();
    [TextArea(3, 10)] public List<string> eng_sentences = new List<string>();
    public List<string> eng_keywords = new List<string>();

    [field: SerializeField] public string InfoText { get; set; }
    [field: SerializeField] public float InfoDisplayTime { get; set; }

    [Space] [Header("TextData References")]
    public List<Interaction> textToUnlock = new List<Interaction>();
    public List<Interaction> textToEnable = new List<Interaction>();
    public List<Interaction> textToDisable = new List<Interaction>();

}