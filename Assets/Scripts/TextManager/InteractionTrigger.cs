﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class InteractionTrigger : MonoBehaviour
{
    public TriggerState triggerState = TriggerState.enabled;
    public Interaction defaultInteraction;
    public Interaction unlockedInteraction;

    [Tooltip("One Shot Interactions will be destroyed when done")]
    public bool isOneShot;
    
    public Interaction ActiveInteraction { get; private set; }

    private Dialogue _displayedDialogue;

    public List<InLevelTrigger> triggers;

    BoxCollider _bc;
    
    [SerializeField] private bool playOnTrigger = false;
    
    [Tooltip("Point to focus camera on")]
    public Transform cameraFocusPoint;
    [Tooltip("Position to lerp camera to")]
    public Transform cameraLerpPosition;
    
    private void Start()
    {
        _bc = GetComponent<BoxCollider>();

        if (playOnTrigger) _bc.isTrigger = true;
        // //Setting layer to Dialogue Layer - moved to OnEnable;
        // gameObject.layer = 6;

        
        ActiveInteraction = defaultInteraction;
        
        if (ActiveInteraction.StartOnAwake)
            InteractionManager.Instance.CurrentInteraction = defaultInteraction;
        
        SetTriggerState(triggerState);    
    }
    public void TriggerDialogue()
    {
        InteractionManager.Instance.LastUsedInteractionTrigger = this;
        DialogueManager.Instance.ChangeTextstate(DialogueManager.TextState.onDisplay,
            ActiveInteraction.assignedDialogue);
        InteractionManager.Instance.SetEndTrigger(triggers);
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
                if (ActiveInteraction.StartOnAwake) TriggerDialogue();
                break;
            case TriggerState.hidden:
                _bc.enabled = false;
                triggerState = tS;
                break;
            case TriggerState.setDefaultText:
                ActiveInteraction = defaultInteraction;
                triggerState = tS;
                if (ActiveInteraction.StartOnAwake) TriggerDialogue();
                break;
            case TriggerState.setUnlockedText:
                ActiveInteraction = unlockedInteraction ? unlockedInteraction : defaultInteraction;
                triggerState = tS;
                break;
        }
    }

    private void OnEnable()
    {
        DialogueManager.EnableTextTrigger += EnableTrigger;
        DialogueManager.DisableTextTrigger += DisableTrigger;
        DialogueManager.UnlockText += UnlockText;
        
        gameObject.layer = 6;
    }

    private void OnDisable()
    {
        DialogueManager.EnableTextTrigger -= EnableTrigger;
        DialogueManager.DisableTextTrigger -= DisableTrigger;
        DialogueManager.UnlockText -= UnlockText;
        
        gameObject.layer = 0;
    }
    
    private void EnableTrigger(List<Interaction> iList)
    {
        List<Dialogue> dList = new List<Dialogue>();
        foreach (var i in iList)
        {
            dList.Add(i.assignedDialogue);
        }

        if (dList.Contains(_displayedDialogue))
        {
            Debug.Log("Enabling Trigger");
            SetTriggerState(TriggerState.enabled);
        }
    }
    private void DisableTrigger(List<Interaction> iList)
    {
        List<Dialogue> dList = new List<Dialogue>();
        foreach (var i in iList)
        {
            dList.Add(i.assignedDialogue);
        }

        if (dList.Contains(_displayedDialogue))
        {
            Debug.Log("Disabling Trigger");
            SetTriggerState(TriggerState.hidden);
        }
    }
    private void UnlockText(Interaction i)
    {
        if (i == unlockedInteraction)
        {
            Debug.Log("Unlocking Text");
            SetTriggerState(TriggerState.setUnlockedText);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && playOnTrigger)
        {
            TriggerDialogue();
        }
    }
    public TriggerState GetTriggerState()
    {
        return triggerState;
    }
}