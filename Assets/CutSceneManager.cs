using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    [field: SerializeField] private CutScene[] cutScenes;
    private CutScene CurrentCutScene => cutScenes[CurrentCutSceneIndex];

    public int CurrentCutSceneIndex { get; private set; }

    public void ResetCutsceneIndex()
    {
        CurrentCutSceneIndex = 0;
    }
    
    public void PlayCurrentCutScene()
    {
        CurrentCutScene.PlayVideo();
    }
}
