using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class InteractionTrigger : MonoBehaviour
{
    public TriggerState triggerState = TriggerState.enabled;
    public Interaction defaultInteraction;
    public Interaction unlockedInteraction;

    private Interaction _activeInteraction;
    BoxCollider _bc;
    
    [SerializeField] private bool playOnTrigger = false;
    
    public Transform[] cameraFocalPoints;

    /**
     * IF in Unity a Singleton is created in awake, it will be destroyed when a new scene is loaded.
     * If you want to keep the Singleton between scenes, you need to use the OnEnable and OnDisable methods.
     * 
     */
    private void OnEnable()
    {
        DialogueManager.EnableTextTrigger += EnableTrigger;
        DialogueManager.DisableTextTrigger += DisableTrigger;
        DialogueManager.UnlockText += UnlockText;
    }

    private void OnDisable()
    {
        DialogueManager.EnableTextTrigger -= EnableTrigger;
        DialogueManager.DisableTextTrigger -= DisableTrigger;
        DialogueManager.UnlockText -= UnlockText;
    }

    private void Start()
    {
        _bc = GetComponent<BoxCollider>();

        if (playOnTrigger) _bc.isTrigger = true;
        //Setting layer to Dialogue Layer;
        gameObject.layer = 6;
        
        _activeInteraction = defaultInteraction;
        SetTriggerState(triggerState);    
    }
    public void TriggerDialogue()
    {
        InteractionManager.Instance.CurrentInteraction = _activeInteraction;
        DialogueManager.Instance.ChangeTextstate(DialogueManager.TextState.onDisplay, _activeInteraction.assignedDialogue);
    }

    public enum TriggerState
    {
        hidden,
        enabled,
        setDefaultText,
        setUnlockedText
    }

    public void SetTriggerState(TriggerState tS)
    {
        switch (tS)
        {
            case TriggerState.enabled:              
                _bc.enabled = true;
                triggerState = tS;
                if (_activeInteraction.StartOnAwake) TriggerDialogue();
                break;
            case TriggerState.hidden:
                _bc.enabled = false;
                triggerState = tS;
                break;
            case TriggerState.setDefaultText:
                _activeInteraction = defaultInteraction;
                triggerState = tS;
                if (_activeInteraction.StartOnAwake) TriggerDialogue();
                break;
            case TriggerState.setUnlockedText:
                _activeInteraction = unlockedInteraction;
                triggerState = tS;
                if (_activeInteraction.StartOnAwake) 
                    TriggerDialogue();
                break;
        }
    }

    private void EnableTrigger(List<Interaction> dList)
    {
        if (dList.Contains(_activeInteraction))
            SetTriggerState(TriggerState.enabled);
    }
    private void DisableTrigger(List<Interaction> dList)
    {
        if (dList.Contains(_activeInteraction))
            SetTriggerState(TriggerState.hidden);
    }
    private void UnlockText(List<Interaction> dList)
    {
        if (dList.Contains(unlockedInteraction))
        {
            SetTriggerState(TriggerState.setUnlockedText);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && playOnTrigger)
        {
            TriggerDialogue();
        }
    }

    public TriggerState GetTriggerState()
    {
        return triggerState;
    }
}
