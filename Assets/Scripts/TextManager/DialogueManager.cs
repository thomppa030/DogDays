using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class DialogueManager : MonoBehaviour
{   
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] float textSpeed = 0.03f;
    private TextState currentTextstate = TextState.none;
    private Queue<string> sentences;
    public static DialogueManager instance;

    public Language selectedLanguage = Language.german;

    public Dialogue currentDialogue { get; set; }

    public enum Language
    {
        german,
        english
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();

        if(dialogueText != null)
            dialogueText.text = "";
    }

    private void DisplayNextSentence()
    {
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

    public static Action<List<Dialogue>> EnableTextTrigger;
    public static Action<List<Dialogue>> DisableTextTrigger;
    public static Action<List<Dialogue>> UnlockText;
    void EndDialogue()
    {
        EnableTextTrigger?.Invoke(currentDialogue.textToEnable);
        DisableTextTrigger?.Invoke(currentDialogue.textToDisable);
        UnlockText?.Invoke(currentDialogue.textToUnlock);

        ChangeTextstate(TextState.none, null);
    }

    private List<string> GetSentence(Dialogue d)
    {

        switch (selectedLanguage)
        {
            case Language.english:
                return d.eng_sentences;
            case Language.german:
                return d.ger_sentences;
            default:
                return d.eng_sentences;

        }
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
                currentDialogue = null;
                actionID = 0;
            }
        }
    }

    private void Update()
    {
        if (currentTextstate != TextState.onDisplay) return;

        if(Input.GetButtonDown("Fire1") && 
            currentDialogue.Actions[actionID] == Dialogue.Action.nextSentence)
        {
            actionID++;
            SetNextAction(currentDialogue, actionID);
        }
    }

    public void ChangeLanguage(int id)
    {        
        selectedLanguage = (Language)id;
        Debug.Log("Changed Language to: " + selectedLanguage);
    }

 
    public void StartDialogue(Dialogue d)
    {
        actionID = 0;
        currentDialogue = d;
        sentences.Clear();

        foreach (string sentence in GetSentence(d))
        {
            sentences.Enqueue(sentence);
        }

        SetNextAction(d, actionID);
    }

    private void EnableText(bool enable)
    {
        Debug.Log("Setting TextDisplay to: " + enable);
        dialogueText.gameObject.SetActive(enable);
    }

    private int actionID = 0;
    private void SetNextAction(Dialogue d, int id)
    {
        switch (d.Actions[id])
        {
            case Dialogue.Action.nextSentence:
                DisplayNextSentence();
                break;
            case Dialogue.Action.enableTextDisplay:
                EnableText(true);
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.disableTextDisplay:
                EnableText(false);
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.wait:
                StartCoroutine(Wait(1f));
                break;
            case Dialogue.Action.endDialogue:
                EndDialogue();
                break;
        }
    }



    IEnumerator Wait(float time)
    {
        Debug.Log($"Wait for {time} seconds.");
        yield return new WaitForSeconds(time);
        actionID++;
        SetNextAction(currentDialogue, actionID);

    }



}

