using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

using CameraShake;

// TODO: Refactor this class to adhere to the Single Responsibility Principle
/**
 * The responsibility of this class is to manage the individual interaction in the game.
 */

public class InteractionManager : MonoBehaviour
{
    //Singleton
    public static InteractionManager Instance;
    
    //TODO: Refactor Interaction and Dialogue to adhere to the Single Responsibility Principle
    public Interaction CurrentInteraction { get; set; }
    
    #region Delegate Declarations 
    public delegate void SwitchCameraFocusDelegate();
    public SwitchCameraFocusDelegate OnSwitchCameraFocus;
    public delegate void ResetCameraFocusDelegate();
    public ResetCameraFocusDelegate OnResetCameraFocus;
    public delegate void DialogueEndDelegate();
    public DialogueEndDelegate OnDialogueEnd;
    public delegate void DialogueStartDelegate(Dialogue d);
    public DialogueStartDelegate OnDialogueStart;
    public delegate void DisableTextDelegate();
    public DisableTextDelegate OnDisableText;
    public delegate void EnableTextDelegate();
    public EnableTextDelegate OnEnableText;
    public delegate void NextSentenceDelegate();
    public NextSentenceDelegate OnNextSentence;
    public delegate void FadeInDelegate();
    public FadeInDelegate OnFadeIn;
    public delegate void FadeOutDelegate();
    public FadeOutDelegate OnFadeOut;
    public delegate void EnableProfileImagedelegate();
    public EnableProfileImagedelegate OnEnableProfileImage;
    public delegate void DisableProfileImagedelegate();
    public DisableProfileImagedelegate OnDisableProfileImage;
    public delegate void WaitforNextSentenceDelegate(float waitingTime);
    public WaitforNextSentenceDelegate OnWaitforNextSentence;
    public delegate void CameraShakeDelegate();
    public CameraShakeDelegate OnCameraShake;
    //TODO: Enum for ProfileImageID would need single class for ProfileImage and StateEnum?
    public delegate void SetProfileImageDelegate(Sprite image);
    public SetProfileImageDelegate OnSetProfileImage;
    public delegate void PlaySoundDelegate(AudioClip clip);
    public PlaySoundDelegate OnPlaySound;
    
    public delegate void DialogueWaitDelegate(float waitingTime);
    public DialogueWaitDelegate OnDialogueWait;
    
    #endregion
    
    private List<InLevelTrigger> _animationTriggers;

    public InteractionTrigger LastUsedInteractionTrigger { get; set; }
    
    public void SetEndTrigger(List<InLevelTrigger> animationTriggers)
    {
        _animationTriggers = animationTriggers;
    }
    
    [field: SerializeField] private PlayerStateMachine PlayerStateMachine { get; set; }
    [field: SerializeField] private CameraStateMachine CameraStateMachine { get; set; }
    
    private Animator _playerAnim;
    [field: SerializeField] public float DefaultWaitingtime { get; private set; } = 1f;

    #region ID's

    [field: SerializeField] public int ActionID { get; set; } = 0;
    private int _waitID = 0;
    private int _audioID = 0;
    private int _profileImageID = 0;
    private int _uiAnimID = 0;
    private int _playerAnimID = 0;
    private int _cameraFocusID = 0;
    #endregion
    
    [SerializeField] private InfoDisplayer infoDisplayer;
    
    private void ShowInfoDisplay()
    {
        infoDisplayer.ShowInfo(CurrentInteraction.assignedDialogue.InfoText,
            CurrentInteraction.assignedDialogue.InfoDisplayTime);
        ActionID++;
        SetNextAction(CurrentInteraction, ActionID);
    }
    private void DisableInfoDisplay()
    {
        infoDisplayer.DisableInfo();
        ActionID++;
        SetNextAction(CurrentInteraction, ActionID);
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(GameState.Instance.GetCurrentState() == GameState.GameStates.Game)
        {
            _playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }
        OnPlaySound += PlaySound;
    }
    
    float sentenceWait = 0f;
    bool waitForSentence = false;
    
    private void SetWaitTimeForSentence(float t)
    {
        sentenceWait = t;
        waitForSentence = true;
        
        DialogueManager.Instance.DisplayNextSentence();
    }
    
    private void Update()
    {
        
        if (Input.GetButtonDown("Fire1") && CurrentInteraction != null &&
            CurrentInteraction.Actions[ActionID] == Interaction.Action.NextSentence)
        {
            ActionID++;
            SetNextAction(CurrentInteraction, ActionID);
        }
        
        if (waitForSentence) WaitForSentence();
    }
    
    private void WaitForSentence()
    {
        if (GameState.Instance.GetCurrentState() != GameState.GameStates.Game)
            return;

        sentenceWait -= Time.deltaTime;
        
        if(sentenceWait <= 0)
        {
            waitForSentence = false;
            // TODO: yeah uhm, this is a bit of a hack, will change it
            SetNextAction(CurrentInteraction, ActionID);
        }
    }

    
    public void SetNextAction(Interaction i, int id)
    {
        Debug.Log($"Play Action {i.Actions[id]} with ID {ActionID}.");
        switch (i.Actions[id])
        {
            case Interaction.Action.NextSentence:
                OnNextSentence?.Invoke();
                break;
            case  Interaction.Action.EnableTextDisplay:
                OnEnableText?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.DisableTextDisplay:
                OnDisableText?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.Wait:
                StartCoroutine(Wait(i.dialogueWaitingTime.waitTime[_waitID]));
                break;
            case Interaction.Action.PlaySfx:
                AudioClip ac = i.dialogueSounds.audioclips[_audioID];
                //TODO: PlaySoundDelegate should be refactored to a single class
                OnPlaySound?.Invoke(ac);
                ActionID++;
                _audioID++;
                SetNextAction(CurrentInteraction, ActionID);
                break;
            case Interaction.Action.FadeIn:
                OnFadeIn?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.FadeOut:
                OnFadeOut?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.EndDialogue:
                OnDialogueEnd?.Invoke();
                ResetIDs();
                TriggerEndTrigger();
                break;
            case Interaction.Action.PlayCharAnim:
                PlayCharacterAnimation();
                _playerAnimID++;
                ActionID++;
                SetNextAction(CurrentInteraction, ActionID);
                break;
            case Interaction.Action.ShowInfoDisplay:
                //TODO: Refactor to a single class
                ShowInfoDisplay();
                break;
            case Interaction.Action.DisableInfoDisplay:
                //TODO: Refactor to a single class
                DisableInfoDisplay();
                break;
            case Interaction.Action.LoadNextScene:
                //TODO: Refactor to a single class
                OnDialogueEnd?.Invoke();
                int sceneID = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
                Debug.Log("Loading scene of index: " + sceneID);
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
                break;
            case Interaction.Action.DisableProfileImage:
                OnDisableProfileImage?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.SetProfileImage:
                OnSetProfileImage?.Invoke(i.dialogueDogFaces.dogFaces[_profileImageID]);
                _profileImageID++;
                ActionID++;
                SetNextAction(CurrentInteraction, ActionID);
                break;
            case Interaction.Action.NextSentenceWithWait:
                SetWaitTimeForSentence(CurrentInteraction.dialogueWaitingTime.waitTime[_waitID]);
                _waitID++;
                ActionID++;
                break;
            case Interaction.Action.ShakeCamera:
                OnCameraShake?.Invoke();
                Debug.LogWarning("Shake Camera needs to be implemented yet!");
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.PlaySfxImmediate:
                //TODO: Delegate and refactor
                AudioClip acImmediate = i.dialogueSounds.audioclips[_audioID];
                PlaySFXImmediate(acImmediate);
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.PlayCharAnimWithWait:
                //TODO: Delegate and refactor
                StartCoroutine(CharAnimWithWait());
                break;
            case Interaction.Action.DisablePlayerMovement:
                //TODO: Delegate and refactor
                DisablePlayerMovement();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.EnablePlayerMovement:
                //TODO: Delegate and refactor
                EnablePlayerMovement();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.SwitchCameraFocus:
                CameraStateMachine.SwitchState(new CameraFocusState(CameraStateMachine,
                    LastUsedInteractionTrigger.cameraLerpPosition, LastUsedInteractionTrigger.cameraFocusPoint));
                ActionID++;
                SetNextAction(i, ActionID);
                break;
            case Interaction.Action.ResetCameraFocus:
                OnResetCameraFocus?.Invoke();
                ActionID++;
                SetNextAction(i, ActionID);
                break;
        }
    }

    private void TriggerEndTrigger()
    {
        foreach (var trigger in _animationTriggers)
        {
            trigger.Trigger();
        }
    }

    void PlaySound(AudioClip ac)
    {
        StartCoroutine(PlaySFX(ac));
    }
    
    //TODO: Refactor to a single class
    AudioSource _audioSource;
    IEnumerator PlaySFX(AudioClip ac)
    {
        float waitTime = ac.length;
        
        Debug.Log($"Playing {ac.name} with ID ${_audioID} and wait time of {waitTime} seconds.");
        
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = ac;
        _audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        _audioSource.Play();
        
        yield return new WaitForSeconds(waitTime);
    }
    
    private void PlaySFXImmediate(AudioClip ac)
    {
        Debug.Log($"Playing {ac.name} with ID ${_audioID}.");

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = ac;
        _audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        _audioSource.Play();

        _audioID++;
    }
    
    //TODO: Refactor to a single class
    void DisablePlayerMovement()
    {
        PlayerStateMachine.SwitchState(new PlayerInteractState(PlayerStateMachine));
    }
    
    void EnablePlayerMovement()
    {
        PlayerStateMachine.SwitchState(new PlayerMovingState(PlayerStateMachine));
    }
    
    private void PlayCharacterAnimation()
    {
        DisablePlayerMovement();
        string ac = CurrentInteraction.dialogueAnimations.characterAnim[_playerAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        _playerAnim.Play(ac);
    }
    
    IEnumerator CharAnimWithWait()
    {
        string ac = CurrentInteraction.dialogueAnimations.characterAnim[_playerAnimID].name.ToString();
        float waitTime = CurrentInteraction.dialogueAnimations.characterAnim[_playerAnimID].length;
        
        Debug.Log("Playing animation: " + ac);
        
        _playerAnim.Play(ac);
        yield return new WaitForSeconds(waitTime);
        
        _playerAnimID++;
        ActionID++;
        SetNextAction(CurrentInteraction, ActionID);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        _waitID++;
        ActionID++;
        SetNextAction(CurrentInteraction, ActionID);
    }

    public void ResetIDs()
    {
        _waitID = 0;
        _audioID = 0;
        ActionID = 0;
        _playerAnimID = 0;
        _profileImageID = 0;
        _cameraFocusID = 0;

        waitForSentence = false;
    }
}