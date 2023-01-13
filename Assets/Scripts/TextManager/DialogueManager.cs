using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour
{   
    [SerializeField] TMP_Text dialogueText;
    private TextState currentTextstate = TextState.none;
    private Queue<string> sentences;
    public static DialogueManager instance;

    public Language selectedLanguage = Language.german;
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

    private void StartDialogue (Dialogue dialogue)
    {
        sentences.Clear();

        foreach(string sentence in GetSentence(dialogue))
        {
            sentences.Enqueue(sentence);
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
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        ChangeTextstate(TextState.none, null);
    }

    private string[] GetSentence(Dialogue d)
    {
        switch (selectedLanguage)
        {
            case Language.german:
                return d.ger_sentences;
            case Language.english:
                return d.eng_sentences;
            default:
                return d.ger_sentences;
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
