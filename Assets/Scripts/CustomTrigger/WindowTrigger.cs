using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WindowTrigger : MonoBehaviour
{
    [field: SerializeField] private GameObject[] brokenWindows;
    [field: SerializeField] private GameObject[] windowTape;

    private void Start()
    {
        BatchDeactivate(brokenWindows);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BatchDeactivate(windowTape);
            BatchActivate(brokenWindows);
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
