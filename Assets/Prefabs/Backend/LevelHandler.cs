using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [field: SerializeField] private Dictionary<IInteractable, String> InteractableObjects { get; set; }
    
    public void LockInteractables()
    {
        foreach (var interActableObject in InteractableObjects)
        {
            interActableObject.Key.Lock();
        }
    }
    
    public void UnlockInteractables(string[] keys)
    {
        for (int i = 0; i < InteractableObjects.Count; i++)
        {
            foreach (var interActableObject in InteractableObjects)
            {
                if (interActableObject.Value.Equals(keys[i]))
                {
                    interActableObject.Key.Unlock();
                }
            }
        }
    }
}
