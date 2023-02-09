﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class DialogueManager : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField] private Animator fader;
    [SerializeField] private InfoDisplayer infoDisplayer;
    [SerializeField] private CutSceneManager cutSceneManager;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image dialogueFrame;
    [SerializeField] Image profileImage;
    [SerializeField] float textSpeed = 0.03f;
    private TextState currentTextstate = TextState.none;
    private Queue<string> sentences;
    AudioSource audioSource;
    public static DialogueManager instance;
    
    [field: SerializeField] private PlayerStateMachine playerStateMachine { get; set; }

    [field: SerializeField] private Image TextFieldImage {get; set; }

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

        sentences = new Queue<string>();
    }

    private void Start()
    {
        if(GameState.Instance.GetCurrentState() == GameState.GameStates.Game)
        {
            playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }

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
        LevelHandler.Instance.LockInteractables();
        InteractComponent interactComponent = GetComponent<InteractComponent>();
        
        EnablePlayerMovement();
        
        if (interactComponent)
        {
            interactComponent.Unlock();
        }
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
            SetNextAction(currentDialogue, actionID);
        }
    }

    public void ChangeLanguage(int id)
    {        
        selectedLanguage = (Language)id;
        Debug.Log("Changed Language to: " + selectedLanguage);
    }

    private void EnableProfileImage(bool enable)
    {
        profileImage.gameObject.SetActive(enable);
    }
 
    public void StartDialogue(Dialogue d)
    {
        EnableProfileImage(false);
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
        dialogueFrame.gameObject.SetActive(enable);
        profileImage.gameObject.SetActive(enable);
    }

    private int actionID = 0;
    private int audioID = 0;
    private int waitID = 0;
    private int profileImageID = 0;
    private int uiAnimID = 0;
   
    [field: SerializeField] public float DefaultWaitingtime { get; private set; } = 1f;

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
                EnableProfileImage(false);
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.wait:
                StartCoroutine(waitID >= d.waitTime.Length ? Wait(DefaultWaitingtime) : Wait(d.waitTime[waitID]));
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
            case Dialogue.Action.disableProfileImage:
                EnableProfileImage(false);
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.setProfileImage:
                SetProfileImage(d.profileImages[profileImageID]);
                break;
            case Dialogue.Action.nextSentenceWithWait:
                SetWaitTimeForSentence(d.waitTime[waitID]);
                waitID++;
                actionID++;
                break;
            case Dialogue.Action.shakeCamera:
                Debug.LogWarning("Shake Camera needs to be implemented yet!");
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.playSFXImmediate:
                AudioClip acImmediate = d.GetAudioClip(audioID);
                PlaySFXImmediate(acImmediate);
                break;
            case Dialogue.Action.playCharAnimWithWait:
                StartCoroutine(CharAnimWithWait());
                break;
            case Dialogue.Action.disablePlayerMovement:
                DisablePlayerMovement();
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.enablePlayerMovement:
                EnablePlayerMovement();
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.TriggerVideoAnimationDay01:
                TriggerVideoAnimationDay01();
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.HideVideoPanelDay01:
                HideVideoPanelDay01();
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.TriggerVideoAnimationDay02:
                TriggerVideoAnimationDay02();
                actionID++;
                SetNextAction(d, actionID);
                break;
            case Dialogue.Action.HideVideoPanelDay02:
                HideVideoPanelDay02();
                actionID++;
                SetNextAction(d, actionID);
                break;
        }
    }

    private void TriggerVideoAnimationDay01()
    {
        Animator anim = cutSceneManager.cutScenesDay01[uiAnimID];
        anim.Play("FadeIn");
        uiAnimID++;
    }

    private void HideVideoPanelDay01()
    {
        Animator anim = cutSceneManager.cutScenesDay01[0];
        Animator anim02 = cutSceneManager.cutScenesDay01[1];
        anim.Play("FadeOut");
        anim02.Play("FadeOut");
    }
    
    private void TriggerVideoAnimationDay02()
    {
        Animator anim = cutSceneManager.cutScenesDay02[uiAnimID];
        anim.Play("FadeIn");
        uiAnimID++;
    }

    private void HideVideoPanelDay02()
    {
        Animator anim = cutSceneManager.cutScenesDay01[0];
        Animator anim02 = cutSceneManager.cutScenesDay02[1];
        Animator anim03 = cutSceneManager.cutScenesDay02[2];
        anim.Play("FadeOut");
        anim02.Play("FadeOut");
        anim03.Play("FadeOut");
    }

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
        EnableProfileImage(true);
        profileImage.sprite = s;
        profileImageID++;
        actionID++;
        SetNextAction(currentDialogue, actionID);
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
        DisablePlayerMovement();
        string ac = currentDialogue.characterAnim[charAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        playerAnim.Play(ac);
        charAnimID++;
        actionID++;
        SetNextAction(currentDialogue, actionID);
    }

    private void PlaySFXImmediate(AudioClip ac)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID}.");

        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        audioSource.Play();

        actionID++;
        audioID++;
        SetNextAction(currentDialogue, actionID);
    }
    IEnumerator CharAnimWithWait()
    {
        string ac = currentDialogue.characterAnim[charAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        playerAnim.Play(ac);
        float waitTime = currentDialogue.characterAnim[charAnimID].length;
        yield return new WaitForSeconds(waitTime);

        charAnimID++;
        actionID++;
        SetNextAction(currentDialogue, actionID);
    }

    IEnumerator PlaySFX(AudioClip ac, float waitTime)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID} and wait time of {waitTime} seconds.");

        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
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
        profileImageID = 0;
        uiAnimID = 0;

        waitForSentence = false;
    }
}

