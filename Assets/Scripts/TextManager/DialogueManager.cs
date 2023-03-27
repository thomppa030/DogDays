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
    public TextState currentTextstate { get; private set; } = TextState.none;
    
    private Queue<string> sentences;
    [field: SerializeField] private PlayerStateMachine playerStateMachine { get; set; }
    [field: SerializeField] private Image TextFieldImage {get; set; }

    public Language selectedLanguage = Language.german;

    public Dialogue _displayedDialogue { get; set; }
    
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

    private void OnEnable()
    {
        SubscribeInteractionEvents();
    }

    private void OnDisable()
    {
        UnsubscribeInteractionEvents();
    }

    private void Start()
    {

        if(dialogueText != null)
            dialogueText.text = "";
        
    }

    private void SubscribeInteractionEvents()
    {
        InteractionManager.Instance.OnDialogueEnd += EndDialogue;
        InteractionManager.Instance.OnDialogueStart += StartDialogue;
        InteractionManager.Instance.OnEnableText += EnableTextFrame;
        InteractionManager.Instance.OnDisableText += DisableTextFrame;
        InteractionManager.Instance.OnNextSentence += DisplayNextSentence;
        InteractionManager.Instance.OnEnableProfileImage += EnableProfileImage;
        InteractionManager.Instance.OnDisableProfileImage += DisableProfileImage;
        InteractionManager.Instance.OnSetProfileImage += SetProfileImage;
    }

    private void UnsubscribeInteractionEvents()
    {
        InteractionManager.Instance.OnDialogueEnd -= EndDialogue;
        InteractionManager.Instance.OnDialogueStart -= StartDialogue;
        InteractionManager.Instance.OnEnableText -= EnableTextFrame;
        InteractionManager.Instance.OnDisableText -= DisableTextFrame;
        InteractionManager.Instance.OnNextSentence -= DisplayNextSentence;
        InteractionManager.Instance.OnEnableProfileImage -= EnableProfileImage;
        InteractionManager.Instance.OnDisableProfileImage -= DisableProfileImage;
        InteractionManager.Instance.OnSetProfileImage -= SetProfileImage;
    }

    public void DisplayNextSentence()
    {
        Debug.Log("Queue size before DisplayingNextSentence: " + sentences.Count);
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
                Keywords = new List<string>(InteractionManager.Instance.CurrentInteraction.assignedDialogue.eng_keywords);
                break;
            case Language.german:
                Keywords = new List<string>(InteractionManager.Instance.CurrentInteraction.assignedDialogue.ger_keywords);
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
    }

    public static Action<List<InteractionTrigger>> EnableTextTrigger;
    public static Action<List<InteractionTrigger>> DisableTextTrigger;
    public static Action<Interaction> UnlockText;

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
                
            }
        }
    }

    private void Update()
    {
        if (currentTextstate != TextState.onDisplay) return;

    }


    public void ChangeLanguage(int id)
    {        
        selectedLanguage = (Language)id;
        Debug.Log("Changed Language to: " + selectedLanguage);
    }

    public void StartDialogue(Dialogue d)
    {
        profileImage.gameObject.SetActive(false);
        InteractionManager.Instance.ResetIDs();
        
        _displayedDialogue = d;
        
        Interaction interaction = InteractionManager.Instance.CurrentInteraction;
        
        Debug.Log("Size of sentences in StartDialogue before Clearing " + sentences.Count);
        sentences.Clear();
        Debug.Log("Size of sentences in StartDialogue after Clearing " + sentences.Count);

        foreach (string sentence in GetSentence(d))
        {
            sentences.Enqueue(sentence);
        }
        Debug.Log("Size of sentences in StartDialogue after Enqueuing " + sentences.Count);

        InteractionManager.Instance.SetNextAction(interaction, InteractionManager.Instance.ActionID);
    }
    
    void EndDialogue()
    {
        ChangeTextstate(TextState.none, null);
        DisableTextFrame();
        EnablePlayerMovement();

        if (InteractionManager.Instance.LastUsedInteractionTrigger.isOneShot)
        {
            Destroy(InteractionManager.Instance.LastUsedInteractionTrigger);
        }
        
        EnableTextTrigger?.Invoke(InteractionManager.Instance.LastUsedInteractionTrigger.interactionToEnable);
        DisableTextTrigger?.Invoke(InteractionManager.Instance.LastUsedInteractionTrigger.interactionToDisable);
        UnlockText?.Invoke(InteractionManager.Instance.LastUsedInteractionTrigger.unlockedInteraction);
    }

    private void EnableTextFrame()
    {
        dialogueText.gameObject.SetActive(true);
        dialogueFrame.gameObject.SetActive(true);
    }
    private void DisableTextFrame()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueFrame.gameObject.SetActive(false);
        DisableProfileImage();
    }

    private void EnablePlayerMovement()
    {
        if (playerStateMachine != null)
            playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
    }
    
    private void DisablePlayerMovement()
    {
        if (playerStateMachine != null)
            playerStateMachine.SwitchState(new PlayerInteractState(playerStateMachine));
    }

    private void SetProfileImage(Sprite s)
    {
        if (!profileImage.gameObject.activeSelf)
        {
            EnableProfileImage();
        }
        profileImage.sprite = s;
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
    }
}

