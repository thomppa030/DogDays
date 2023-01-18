using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour
{   
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] float textSpeed = 0.03f;
    private TextState currentTextstate = TextState.none;
    private Queue<string> sentences;
    public static DialogueManager instance;

    public Language selectedLanguage = Language.german;

    [Header("Insert all Dialogue Data here!")]
    public List<Dialogue> DialogueData = new List<Dialogue>();
    public enum Language
    {
        german,
        english
    }

    private void Awake()
    {
        instance = this;

        if(DialogueData.Count <= 0)
        {
            Debug.LogWarning("No dialogue Data found for reseting booleans.");
        }
    }

    private void Start()
    {
        sentences = new Queue<string>();

        if(dialogueText != null)
            dialogueText.text = "";

        //Setting booleans of dialogues to false;
        foreach(Dialogue d in DialogueData)
        {
            d.newTextUnlocked = false;
        }
    }

    private void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();

        foreach(string sentence in GetSentence(dialogue))
        {
            sentences.Enqueue(sentence);
        }

        foreach(Dialogue d in dialogue.UnlockableDialogues)
        {
            d.newTextUnlocked = true;
        }
        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        dialogueText.text = sentence;
        
        StartCoroutine(TypeSentence(sentence));

    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        ChangeTextstate(TextState.none, null);
    }

    private List<string> GetSentence(Dialogue d)
    {
        List<string> sentence = new List<string>();
        switch (selectedLanguage)
        {
            case Language.german:
                if (!d.newTextUnlocked)
                    sentence = d.ger_default;
                else
                    sentence = d.ger_unlocked;
                break;
            case Language.english:
                if (!d.newTextUnlocked)
                    sentence = d.eng_default;
                else
                    sentence = d.eng_unlocked;
                break;
            default:
                return d.ger_default;
        }

        return sentence;
    }

    public enum TextState
    {
        none,
        onDisplay,
    }

    public void ChangeTextstate(TextState tS, Dialogue d)
    {
        if (tS == currentTextstate) return;

        //Current State: None
        if(currentTextstate == TextState.none)
        {
            //Start Dialogue
            if (tS == TextState.onDisplay)
            {
                dialogueText.gameObject.SetActive(true);
                currentTextstate = tS;
                StartDialogue(d);
            }
        }
        else if(currentTextstate == TextState.onDisplay)
        {
            //End Dialogue
            if (tS == TextState.none)
            {
                dialogueText.gameObject.SetActive(false);
                dialogueText.text = "";
                currentTextstate = tS;
            }
        }
    }

    private void Update()
    {
        if (currentTextstate != TextState.onDisplay) return;

        if(Input.GetButtonDown("Fire1"))
        {
            DisplayNextSentence();
        }
    }

    public void ChangeLanguage(int id)
    {        
        selectedLanguage = (Language)id;
        Debug.Log("Changed Language to: " + selectedLanguage);
    }
}
