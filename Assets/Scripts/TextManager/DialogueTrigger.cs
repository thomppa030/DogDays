﻿using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public void TriggerDialogue()
    {
        DialogueManager.instance.ChangeTextstate(DialogueManager.TextState.onDisplay, dialogue);
    }


    //TESTING PURPOSE ONLY
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerDialogue();
        }
    }
    */
}
