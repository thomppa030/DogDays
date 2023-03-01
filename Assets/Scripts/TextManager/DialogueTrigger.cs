using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DialogueTrigger : MonoBehaviour
{
    public TriggerState triggerState = TriggerState.enabled;
    public Interaction defaultDialogue;
    public Interaction unlockedDialogue;

    private Interaction displayedDialogue;
    BoxCollider bc;
    
    [SerializeField] private bool playOnTrigger = false;

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
        bc = GetComponent<BoxCollider>();

        if (playOnTrigger) bc.isTrigger = true;
        //Setting layer to Dialogue Layer;
        gameObject.layer = 6;
        

        displayedDialogue = defaultDialogue;
        SetTriggerState(triggerState);    
    }
    public void TriggerDialogue()
    {
        DialogueManager.Instance.ChangeTextstate(DialogueManager.TextState.onDisplay, displayedDialogue.AssignedDialogue);
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
                bc.enabled = true;
                triggerState = tS;
                if (displayedDialogue.startOnAwake) TriggerDialogue();
                break;
            case TriggerState.hidden:
                bc.enabled = false;
                triggerState = tS;
                break;
            case TriggerState.setDefaultText:
                displayedDialogue = defaultDialogue;
                triggerState = tS;
                if (displayedDialogue.startOnAwake) TriggerDialogue();
                break;
            case TriggerState.setUnlockedText:
                displayedDialogue = unlockedDialogue;
                triggerState = tS;
                if (displayedDialogue.startOnAwake) 
                    TriggerDialogue();
                break;
        }
    }

    private void EnableTrigger(List<Interaction> dList)
    {
        if (dList.Contains(displayedDialogue))
            SetTriggerState(TriggerState.enabled);
    }
    private void DisableTrigger(List<Interaction> dList)
    {
        if (dList.Contains(displayedDialogue))
            SetTriggerState(TriggerState.hidden);
    }
    private void UnlockText(List<Interaction> dList)
    {
        
        if (dList.Contains(unlockedDialogue))
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
