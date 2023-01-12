using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Start()
    {
        //Setting layer to Dialogue Layer;
        gameObject.layer = 6;
    }
    public void TriggerDialogue()
    {
        DialogueManager.instance.ChangeTextstate(DialogueManager.TextState.onDisplay, dialogue);
    }
}
