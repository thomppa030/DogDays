using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator fader;
    [SerializeField] private InfoDisplayer infoDisplayer;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] float textSpeed = 0.03f;
    private TextState currentTextstate = TextState.none;
    private Queue<string> sentences;
    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        sentences = new Queue<string>();

        if(dialogueText != null)
            dialogueText.text = "";
    }

    public TextState GetCurrentTextState()
    {
        return currentTextstate;
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
        Debug.Log("Change dialogue state from " + currentTextstate + " to " + tS);
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

                ResetIDs();

                EnableTextTrigger?.Invoke(currentDialogue.textToEnable);
                DisableTextTrigger?.Invoke(currentDialogue.textToDisable);
                UnlockText?.Invoke(currentDialogue.textToUnlock);
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
        ResetIDs();
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
    private int audioID = 0;
    private int waitID = 0;
    private int charAnimID = 0;
    private void SetNextAction(Dialogue d, int id)
    {
        Debug.Log($"Play Action {d.Actions[id]} with ID {actionID}.");
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
                StartCoroutine(Wait(d.waitTime[waitID]));
                break;
            case Dialogue.Action.playSFX:
                AudioClip ac = d.GetAudioClip(audioID);
                float waitTime = ac.length;
                StartCoroutine(PlaySFX(ac, waitTime));
                break;
            case Dialogue.Action.fadeIn:
                StartCoroutine(PlayFadeAnimation("FadeIn"));
                break;
            case Dialogue.Action.fadeOut:
                StartCoroutine(PlayFadeAnimation("FadeOut"));
                break;
            case Dialogue.Action.endDialogue:
                EndDialogue();
                break;
            case Dialogue.Action.playCharAnim:
                PlayCharacterAnimation();
                break;
            case Dialogue.Action.showInfoDisplay:
                ShowInfoDisplay();
                break;
            case Dialogue.Action.disableInfoDisplay:
                DisableInfoDisplay();
                break;
            case Dialogue.Action.loadNextScene:
                EndDialogue();
                int sceneID = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
                Debug.Log("Loading scene of index: " + sceneID);
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
                break;
        }
    }

    private void ShowInfoDisplay()
    {
        infoDisplayer.ShowInfo(currentDialogue.InfoText, currentDialogue.InfoDisplayTime);
        actionID++;
        SetNextAction(currentDialogue, actionID);
    }
    private void DisableInfoDisplay()
    {
        infoDisplayer.DisableInfo();
        actionID++;
        SetNextAction(currentDialogue, actionID);
    }

    private void PlayCharacterAnimation()
    {
        string ac = currentDialogue.characterAnim[charAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        playerAnim.Play(ac);
        charAnimID++;
        actionID++;
        SetNextAction(currentDialogue, actionID);

    }


    IEnumerator PlaySFX(AudioClip ac, float waitTime)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID} and wait time of {waitTime} seconds.");
        audioSource.clip = ac;      
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
        actionID++;
        audioID++;
        SetNextAction(currentDialogue, actionID);
    }
    IEnumerator PlayFadeAnimation(string animName)
    {
        fader.Play(animName);
        yield return new WaitForSeconds(1);
        actionID++;
        SetNextAction(currentDialogue, actionID);
    }

    IEnumerator Wait(float time)
    {
        Debug.Log($"Wait for {time} seconds.");
        yield return new WaitForSeconds(time);
        actionID++;
        waitID++;
        SetNextAction(currentDialogue, actionID);

    }

    private void ResetIDs()
    {
        waitID = 0;
        audioID = 0;
        actionID = 0;
        charAnimID = 0;
    }


}

