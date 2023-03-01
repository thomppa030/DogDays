using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image dialogueFrame;
    
    [Tooltip("Container for the Face of the Dog in the Dialogue")]
    [SerializeField] Image profileImage;
    [SerializeField] float textSpeed = 0.03f;
    private TextState currentTextstate = TextState.none;
    
    private Queue<string> sentences;
    [field: SerializeField] private PlayerStateMachine playerStateMachine { get; set; }
    [field: SerializeField] private Image TextFieldImage {get; set; }

    public Language selectedLanguage = Language.german;

    public enum Language
    {
        german,
        english
    }

    private void Awake()
    {
        Instance = this;

        sentences = new Queue<string>();
    }

    private void Start()
    {

        if(dialogueText != null)
            dialogueText.text = "";
        
        InitializeInteractionEvents();
    }

    private void InitializeInteractionEvents()
    {
        InteractionManager.Instance.OnDialogueEnd += EndDialogue;
        InteractionManager.Instance.OnDialogueStart += StartDialogue;
        InteractionManager.Instance.OnEnableText += EnableText;
        InteractionManager.Instance.OnDisableText += DisableText;
        InteractionManager.Instance.OnNextSentence += DisplayNextSentence;
    }

    private void DisplayNextSentence()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        dialogueText.text = sentence;
        
        StartCoroutine(TypeSentence(sentence));
    }
    
    void CheckForKeyword(string sentence)
    {
        // Divide sentence into words
        string[] words = sentence.Split(' ');

        List<string> Keywords = new List<string>();
        
        switch (selectedLanguage)
        {
            case Language.english:
                Keywords = new List<string>(InteractionManager.Instance.currentDialogue.eng_keywords);
                break;
            case Language.german:
                Keywords = new List<string>(InteractionManager.Instance.currentDialogue.ger_keywords);
                break;
        }
        
        // Loop through words
        foreach (string word in words)
        {
            // Check if word is a keyword
            if (Keywords.Contains(word))
            {
                // Replace word with bolded version
                sentence = sentence.Replace(word, "<b><color=red>" + word + "</color></b>");
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        
        CheckForKeyword(dialogueText.text);
    }

    public static Action<List<Dialogue>> EnableTextTrigger;
    public static Action<List<Dialogue>> DisableTextTrigger;
    public static Action<List<Dialogue>> UnlockText;

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
        Debug.Log("Change dialogue state from " + currentTextstate + " to " + tS);
        if (tS == currentTextstate) return;

        //Current State: None
        if(currentTextstate == TextState.none)
        {
            //Start Dialogue
            if (tS == TextState.onDisplay)
            {
                TextFieldImage.gameObject.SetActive(true);
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
                TextFieldImage.gameObject.SetActive(false);
                dialogueText.text = "";
                currentTextstate = tS;

                ResetIDs();

                EnableTextTrigger?.Invoke(InteractionManager.Instance.currentDialogue.textToEnable);
                DisableTextTrigger?.Invoke(InteractionManager.Instance.currentDialogue.textToDisable);
                UnlockText?.Invoke(InteractionManager.Instance.currentDialogue.textToUnlock);
            }
        }
    }

    private void Update()
    {
        if (currentTextstate != TextState.onDisplay) return;

        //TODO: Change to Inputaction

        if (waitForSentence) WaitForSentence();
    }


    float sentenceWait = 0f;
    bool waitForSentence = false;
    private void SetWaitTimeForSentence(float t)
    {
        sentenceWait = t;
        waitForSentence = true;

        DisplayNextSentence();

    }

    private void WaitForSentence()
    {
        if (currentTextstate == TextState.none) return;
        if (GameState.Instance.GetCurrentState() != GameState.GameStates.Game) return;

        sentenceWait -= Time.deltaTime;
        if(sentenceWait <= 0)
        {
            waitForSentence = false;
            // TODO: yeah uhm, this is a bit of a hack, will change it
            InteractionManager.Instance.SetNextAction(InteractionManager.Instance.currentDialogue, InteractionManager.Instance._actionID);
        }
    }

    public void ChangeLanguage(int id)
    {        
        selectedLanguage = (Language)id;
        Debug.Log("Changed Language to: " + selectedLanguage);
    }

    public void StartDialogue(Dialogue d)
    {
        profileImage.gameObject.SetActive(false);
        ResetIDs();
        InteractionManager.Instance.currentDialogue = d;
    
        sentences.Clear();

        foreach (string sentence in GetSentence(d))
        {
            sentences.Enqueue(sentence);
        }

        InteractionManager.Instance.SetNextAction(d, InteractionManager.Instance._actionID);
    }
    
    void EndDialogue()
    {
        ChangeTextstate(TextState.none, null);
        LevelHandler.Instance.LockInteractables();
        InteractComponent interactComponent = GetComponent<InteractComponent>();
        
        EnablePlayerMovement();
        
        if (interactComponent)
        {
            interactComponent.Unlock();
        }
    }

    private void EnableText()
    {
        dialogueText.gameObject.SetActive(true);
        dialogueFrame.gameObject.SetActive(true);
        profileImage.gameObject.SetActive(true);
    }
    private void DisableText()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueFrame.gameObject.SetActive(false);
        profileImage.gameObject.SetActive(false);
    }

    private int audioID = 0;
    private int waitID = 0;
    private int profileImageID = 0;
    private int uiAnimID = 0;
   

    private int charAnimID = 0;


    private void EnablePlayerMovement()
    {
        if (playerStateMachine != null)
            playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
    }

    private void DisablePlayerMovement()
    {
        if (playerStateMachine != null)
            playerStateMachine.SwitchState(new PlayerReadingState(playerStateMachine));
    }

    private void SetProfileImage(Sprite s)
    {
        profileImage.gameObject.SetActive(true);
        profileImage.sprite = s;
        profileImageID++;
    }
    
    private void EnableProfileImage()
    {
        profileImage.gameObject.SetActive(true);
    }
    
    private void DisableProfileImage()
    {
        profileImage.gameObject.SetActive(false);
    }
    
    private void ResetIDs()
    {
        waitID = 0;
        audioID = 0;
        charAnimID = 0;
        profileImageID = 0;

        waitForSentence = false;
    }
}

