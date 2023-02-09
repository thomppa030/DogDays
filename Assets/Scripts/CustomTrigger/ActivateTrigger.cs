using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActivateTrigger : MonoBehaviour
{
    [field: SerializeField] private GameObject[] ActiveAfter;
    [field: SerializeField] private GameObject[] ActiveBefore;

    private void Start()
    {
        BatchDeactivate(ActiveAfter);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BatchDeactivate(ActiveBefore);
            BatchActivate(ActiveAfter);
        }
    }
    
    private void BatchDeactivate(GameObject[] arr)
    {
        foreach (var obj in arr)
        {
            obj.SetActive(false);
        }
    }
    private void BatchActivate(GameObject[] arr)
    {
        foreach (var obj in arr)
        {
            obj.SetActive(true);
        }
    }
}
