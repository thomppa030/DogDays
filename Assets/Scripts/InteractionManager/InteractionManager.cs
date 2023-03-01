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
    public delegate void SetProfileImageDelegate(Sprite Image);
    public SetProfileImageDelegate OnSetProfileImage;
    public delegate void PlaySoundDelegate(AudioClip clip);
    public PlaySoundDelegate OnPlaySound;
    
    public delegate void DialogueWaitDelegate(float waitingTime);
    public DialogueWaitDelegate OnDialogueWait;
    
    #endregion
    
    private Animator playerAnim;
    [field: SerializeField] public float DefaultWaitingtime { get; private set; } = 1f;

    public int _actionID = 0;

    [SerializeField] private InfoDisplayer infoDisplayer;
    
    private void ShowInfoDisplay()
    {
        infoDisplayer.ShowInfo(currentInteraction.assignedDialogue.InfoText,
            currentInteraction.assignedDialogue.InfoDisplayTime);
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
        if (currentInteraction.Actions[_actionID] == Interaction.Action.NextSentence)
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
    
    private bool waitForSentence = false;
    
    public void SetNextAction(Interaction i, int id)
    {
        Debug.Log($"Play Action {i.Actions[id]} with ID {_actionID}.");
        switch (i.Actions[id])
        {
            case Interaction.Action.NextSentence:
                OnNextSentence?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case  Interaction.Action.EnableTextDisplay:
                OnEnableText?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.DisableTextDisplay:
                OnDisableText?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.Wait:
                OnDialogueWait?.Invoke(i.dialogueWaitingTime.waitTime[waitID]);
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.PlaySfx:
                AudioClip ac = i.dialogueSounds.audioclips[audioID];
                //TODO: PlaySoundDelegate should be refactored to a single class
                OnPlaySound?.Invoke(ac);
                _actionID++;
                audioID++;
                SetNextAction(currentInteraction, _actionID);
                break;
            case Interaction.Action.FadeIn:
                OnFadeIn?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.FadeOut:
                OnFadeOut?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.EndDialogue:
                OnDialogueEnd?.Invoke();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.PlayCharAnim:
                PlayCharacterAnimation();
                playerAnimID++;
                _actionID++;
                SetNextAction(currentInteraction, _actionID);
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
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.SetProfileImage:
                OnSetProfileImage?.Invoke(i.dialogueDogFaces.dogFaces[profileImageID]);
                _actionID++;
                SetNextAction(currentInteraction, _actionID);
                break;
            case Interaction.Action.NextSentenceWithWait:
                OnWaitforNextSentence?.Invoke(i.dialogueWaitingTime.waitTime[waitID]);
                waitID++;
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.ShakeCamera:
                OnCameraShake?.Invoke();
                Debug.LogWarning("Shake Camera needs to be implemented yet!");
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.PlaySfxImmediate:
                //TODO: Delegate and refactor
                AudioClip acImmediate = i.dialogueSounds.audioclips[audioID];
                PlaySFXImmediate(acImmediate);
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.PlayCharAnimWithWait:
                //TODO: Delegate and refactor
                StartCoroutine(CharAnimWithWait());
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.DisablePlayerMovement:
                //TODO: Delegate and refactor
                DisablePlayerMovement();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
            case Interaction.Action.EnablePlayerMovement:
                //TODO: Delegate and refactor
                EnablePlayerMovement();
                _actionID++;
                SetNextAction(i, _actionID);
                break;
                //TODO: Make this bearable
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
    
    IEnumerator PlaySFX(AudioClip ac)
    {
        float waitTime = ac.length;
        
        Debug.Log($"Playing {ac.name} with ID ${audioID} and wait time of {waitTime} seconds.");
        
        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        audioSource.Play();
        yield return new WaitForSeconds(waitTime);
    }
    
    private void PlaySFXImmediate(AudioClip ac)
    {
        Debug.Log($"Playing {ac.name} with ID ${audioID}.");

        audioSource.clip = ac;
        audioSource.volume = MenuHandler.singleton.GetSoundVolume();
        audioSource.Play();

        audioID++;
    }
    
    void DisablePlayerMovement()
    {
        playerStateMachine.SwitchState(new PlayerReadingState(playerStateMachine));
    }
    
    void EnablePlayerMovement()
    {
        playerStateMachine.SwitchState(new PlayerMovingState(playerStateMachine));
    }
    
    
    private void PlayCharacterAnimation()
    {
        DisablePlayerMovement();
        string ac = currentInteraction.dialogueAnimations.characterAnim[playerAnimID].name.ToString();
        Debug.Log("Playing animation: " + ac);
        playerAnim.Play(ac);
    }

    AudioSource audioSource;
    
    IEnumerator CharAnimWithWait()
    {
        string ac = currentInteraction.dialogueAnimations.characterAnim[playerAnimID].name.ToString();
        float waitTime = currentInteraction.dialogueAnimations.characterAnim[playerAnimID].length;
        
        Debug.Log("Playing animation: " + ac);
        
        playerAnim.Play(ac);
        yield return new WaitForSeconds(waitTime);
        
        playerAnimID++;
    }


    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
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
    private void ResetIDs()
    {
        waitID = 0;
        audioID = 0;
        playerAnimID = 0;
        profileImageID = 0;

        waitForSentence = false;
    }
}
