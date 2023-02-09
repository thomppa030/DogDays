using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "NewText", order = 1)]
[HelpURL("https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private string Title = "default Title";

    [field: SerializeField] public bool startOnAwake { get; set; }
    [field: SerializeField] public Action[] Actions { get; set; }
    [Space]
    [Header("Sentences")]
    [TextArea(3, 10)]
    public List<string> ger_sentences = new List<string>();
    [TextArea(3, 10)]
    public List<string> eng_sentences = new List<string>();

    [field: SerializeField] public string InfoText { get; set; }
    [field: SerializeField] public float InfoDisplayTime { get; set; }

    [field: SerializeField] public float[] waitTime { get; set; }
    [field: SerializeField] public AnimationClip[] characterAnim { get; set; }
    [field: SerializeField] public Sprite[] profileImages { get; set; }
    [Space]
    [Header("SFX")]
    [SerializeField] private AudioClip[] Audioclips;
    [Space]
    [Header("TextData References")]
    public List<Dialogue> textToUnlock = new List<Dialogue>();
    public List<Dialogue> textToEnable = new List<Dialogue>();
    public List<Dialogue> textToDisable = new List<Dialogue>();

    public List<GameObject> VideosToPlayInOrder = new List<GameObject>();
    
    public enum Action
    {
        nextSentence,
        enableTextDisplay,
        disableTextDisplay,
        wait,
        fadeIn,
        fadeOut,
        shakeCamera,
        playSFX,
        endDialogue,
        loadNextScene,
        playCharAnim,
        showInfoDisplay,
        disableInfoDisplay,
        setProfileImage,
        disableProfileImage,
        nextSentenceWithWait,
        playSFXImmediate,
        playCharAnimWithWait,
        playNextUIAnimation,
        fadeOutUIAnimation,
        disablePlayerMovement,
        enablePlayerMovement,
    }

    public AudioClip GetAudioClip(int id)
    {
        return Audioclips[id];
    }

    
}
