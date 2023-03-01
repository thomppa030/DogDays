using System;
using System.Collections;
using UnityEngine;

// TODO: Refactor this class to adhere to the Single Responsibility Principle
/**
 * The responsibility of this class is to manage the individual interaction in the game.
 */

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    
    [field: SerializeField] private PlayerStateMachine playerStateMachine { get; }

    //TODO: Refactor Interaction and Dialogue to adhere to the Single Responsibility Principle
    public Interaction currentInteraction { get; set; }
    
    #region Delegate Declarations 

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
    public delegate void FadeInDelegate(string animationName);
    public FadeInDelegate OnFadeIn;
    public delegate void FadeOutDelegate(string animationName);
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
    public delegate void SetProfileImageDelegate(Sprite Image);
    public SetProfileImageDelegate OnSetProfileImage;

    #endregion
    
    private Animator playerAnim;
    [field: SerializeField] public float DefaultWaitingtime { get; private set; } = 1f;

    public int _actionID = 0;

    [SerializeField] private InfoDisplayer infoDisplayer;
    
    private void ShowInfoDisplay()
    {
        infoDisplayer.ShowInfo(currentInteraction.AssignedDialogue.InfoText,
            currentInteraction.AssignedDialogue.InfoDisplayTime);
        _actionID++;
        SetNextAction(currentInteraction, _actionID);
    }
    private void DisableInfoDisplay()
    {
        infoDisplayer.DisableInfo();
        _actionID++;
        SetNextAction(currentInteraction, _actionID);
    }
    
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        if(GameState.Instance.GetCurrentState() == GameState.GameStates.Game)
        {
            playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        }
        
        playerStateMachine.InputReader.TriggerInteractionEvent += TriggerNextAction;
    }
    
    private void TriggerNextAction()
    {
        if (currentInteraction.Actions[_actionID] == Interaction.Action.nextSentence)
        {
            _actionID++;
            SetNextAction(currentInteraction, _actionID);
        }
    }

    private int waitID = 0;
    private int audioID = 0;
    private int profileImageID = 0;
    private int uiAnimID = 0;
    private int playerAnimID = 0;
    
    public void SetNextAction(Interaction i, int id)
    {
        Debug.Log($"Play Action {i.Actions[id]} with ID {_actionID}.");
        switch (i.Actions[id])
        {
            case Interaction.Action.nextSentence:
                OnNextSentence?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case  Interaction.Action.enableTextDisplay:
                OnEnableText?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.disableTextDisplay:
                OnDisableText?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.wait:
                StartCoroutine(waitID >= i.waitTime.Length
                    ? Wait(DefaultWaitingtime)
                    : Wait(i.waitTime[waitID]));
                break;
            case Interaction.Action.playSFX:
                AudioClip ac = i.GetAudioClip(audioID);
                float waitTime = ac.length;
                StartCoroutine(PlaySFX(ac, waitTime));
                break;
            case Interaction.Action.fadeIn:
                OnFadeIn?.Invoke("FadeIn");
                StartCoroutine(PlayFadeAnimation("FadeIn"));
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.fadeOut:
                OnFadeOut?.Invoke("FadeOut");
                StartCoroutine(PlayFadeAnimation("FadeOut"));
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.endDialogue:
                OnDialogueEnd?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.playCharAnim:
                PlayCharacterAnimation();
                playerAnimID++;
                _actionID++;
                SetNextAction(currentInteraction, _actionID);
                break;
            case Interaction.Action.showInfoDisplay:
                ShowInfoDisplay();
                break;
            case Interaction.Action.disableInfoDisplay:
                DisableInfoDisplay();
                break;
            case Interaction.Action.loadNextScene:
                OnDialogueEnd?.Invoke();
                int sceneID = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
                Debug.Log("Loading scene of index: " + sceneID);
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
                break;
            case Interaction.Action.disableProfileImage:
                OnDisableProfileImage?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.setProfileImage:
                OnSetProfileImage?.Invoke(i.profileImages[profileImageID]);
                _actionID++;
                SetNextAction(currentInteraction, _actionID);
                break;
            case Interaction.Action.nextSentenceWithWait:
                OnWaitforNextSentence?.Invoke(i.waitTime[waitID]);
                waitID++;
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.shakeCamera:
                OnCameraShake?.Invoke();
                Debug.LogWarning("Shake Camera needs to be implemented yet!");
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.playSFXImmediate:
                AudioClip acImmediate = i.GetAudioClip(audioID);
                PlaySFXImmediate(acImmediate);
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.playCharAnimWithWait:
                StartCoroutine(CharAnimWithWait());
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.disablePlayerMovement:
                DisablePlayerMovement();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.enablePlayerMovement:
                EnablePlayerMovement();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.TriggerVideoAnimationDay01:
                TriggerVideoAnimationDay01();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.HideVideoPanel01Day01:
                HideVideoPanel01Day01();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.HideVideoPanel02Day01:
                HideVideoPanel02Day01();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.TriggerVideoAnimationDay02:
                TriggerVideoAnimationDay02();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.HideVideoPanelDay02:
                HideVideoPanelDay02();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.ResetAnimID:
                uiAnimID = 0;
                _actionID++;
                SetNextAction(i, _actionID);
                break;
        }
    }
    
    void DisablePlayerMovement()
    {
        playerStateMachine.SwitchState(new PlayerReadingState(playerStateMachine));
    }
    
    void EnablePlayerMovement()
    {
        playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
    }
    
    [SerializeField] private Animator fader;
    IEnumerator PlayFadeAnimation(string animName)
    {
        fader.Play(animName);
        yield return new WaitForSeconds(1);
    }
    
    private void PlayCharacterAnimation()
    {
        DisablePlayerMovement();
        string ac = currentInteraction.characterAnim[playerAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        playerAnim.Play(ac);
    }

    AudioSource audioSource;
    private void PlaySFXImmediate(AudioClip ac)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID}.");

        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        audioSource.Play();

        audioID++;
    }
    
    IEnumerator CharAnimWithWait()
    {
        string ac = currentInteraction.characterAnim[playerAnimID].name.ToString();
        float waitTime = currentInteraction.characterAnim[playerAnimID].length;
        
        Debug.Log("Playing animation: " + ac);
        
        playerAnim.Play(ac);
        yield return new WaitForSeconds(waitTime);
        
        playerAnimID++;
    }

    IEnumerator PlaySFX(AudioClip ac, float waitTime)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID} and wait time of {waitTime} seconds.");

        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
        _actionID++;
        audioID++;
        SetNextAction(currentInteraction, _actionID);
    }

    IEnumerator Wait(float time)
    {
        Debug.Log($"Wait for {time} seconds.");
        yield return new WaitForSeconds(time);
    }

    private void ResetIDs()
    {
        _actionID = 0;
    }
    
    public CutSceneManager cutSceneManager;
    
    private void TriggerVideoAnimationDay01()
    {
        Animator anim = cutSceneManager.cutScenesDay01[uiAnimID];
        anim.Play("FadeIn");
        uiAnimID++;
    }

    private void HideVideoPanel01Day01()
    {
        Animator anim = cutSceneManager.cutScenesDay01[0];
        anim.Play("FadeOut");
    }
    
    private void HideVideoPanel02Day01()
    {
        Animator anim = cutSceneManager.cutScenesDay01[1];
        anim.Play("FadeOut");
    }
    
    private void TriggerVideoAnimationDay02()
    {
        Animator anim = cutSceneManager.cutScenesDay02[uiAnimID];
        anim.Play("FadeIn");
        uiAnimID++;
    }

    private void HideVideoPanelDay02()
    {
        Animator anim = cutSceneManager.cutScenesDay02[0];
        Animator anim02 = cutSceneManager.cutScenesDay02[1];
        Animator anim03 = cutSceneManager.cutScenesDay02[2];
        anim.Play("FadeOut");
        anim02.Play("FadeOut");
        anim03.Play("FadeOut");
    }
}
