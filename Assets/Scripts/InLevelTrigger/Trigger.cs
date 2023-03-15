using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class InLevelTrigger : MonoBehaviour
{
    public bool isOneShot;
    public abstract void Trigger();
}
