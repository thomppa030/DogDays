using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "NewText", order = 1)]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html")]
public class Dialogue : ScriptableObject
{
    [Header("Default Text")]
    [TextArea(3,10)]
    public List<string> ger_default = new List<string>();
    [TextArea(3, 10)]
    public List<string> eng_default = new List<string>();

    [Header("Unlocked Text")]
    [TextArea(3, 10)]
    public List<string> ger_unlocked = new List<string>();
    [TextArea(3, 10)]
    public List<string> eng_unlocked = new List<string>();

    [Header("Unlocking Dialogues for")]
    public List<Dialogue> UnlockableDialogues = new List<Dialogue>();
    public bool newTextUnlocked = false;

    



}
