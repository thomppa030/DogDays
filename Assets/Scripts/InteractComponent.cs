using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    [field: SerializeField] private InteractComponent[] UnlockableInteractComponents { get; set; }

    private InteractionTrigger InteractionTrigger { get; set; }

    private void Start()
    {
        InteractionTrigger = GetComponent<InteractionTrigger>();
    }

    public void Lock()
    {
        if (InteractionTrigger)
        {
            InteractionTrigger.triggerState = InteractionTrigger.TriggerState.hidden;
        }
    }

    public void Unlock()
    {
        foreach (var interactComponent in UnlockableInteractComponents)
        {
            interactComponent.InteractionTrigger.triggerState = InteractionTrigger.TriggerState.enabled;
        }
    }
}
