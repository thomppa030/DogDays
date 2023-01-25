using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [field: SerializeField] private InteractComponent[] UnlockableInteractComponents { get; set; }

    private DialogueTrigger DialogueTrigger { get; set; }

    private void Start()
    {
        DialogueTrigger = GetComponent<DialogueTrigger>();
    }

    public void Lock()
    {
        if (DialogueTrigger)
        {
            DialogueTrigger.triggerState = DialogueTrigger.TriggerState.hidden;
        }
    }

    public void Unlock()
    {
        foreach (var interactComponent in UnlockableInteractComponents)
        {
            interactComponent.DialogueTrigger.triggerState = DialogueTrigger.TriggerState.enabled;
        }
    }
}
