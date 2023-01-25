using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    private InteractComponent[] InteractableObjects { get; set; }
    
    public static LevelHandler Instance { get; private set; }
    
    private void Awake()
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
        InteractableObjects = FindObjectsOfType<InteractComponent>();
    }

    public void LockInteractables()
    {
        foreach (var interActableObject in InteractableObjects)
        {
            interActableObject.Lock();
        }
    }
}
