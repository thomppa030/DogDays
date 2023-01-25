using UnityEngine;

public class TextRayCast : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] float rayDistance = 1f;
    [Header("Icons")]
    [SerializeField] private GameObject interactionIcon;
    
    private PlayerStateMachine playerStateMachine;

    private Camera cam;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        
        EnableIcon(false);
        cam = Camera.main;
    }

    private void Update()
    {
        Raycast();

        if (interactionIcon.activeSelf) interactionIcon.transform.LookAt(cam.transform);
    }

    private void Raycast()
    {
        RaycastHit hit;

        //ObjectLayers
        int layerMask = 1 << 6;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask))
        {
            EnableIcon(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerStateMachine != null)
                    playerStateMachine.SwitchState(new PlayerReadingState(playerStateMachine));

                Debug.Log("Interacting with: " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.TryGetComponent<DialogueTrigger>(out DialogueTrigger dT))
                {
                    dT.TriggerDialogue();
                }
                else
                {
                    Debug.LogError("No Dialogue attached to: " + hit.collider.gameObject.name);
                }
            }
        }
        else
        {
            EnableIcon(false);
        }       
    }

    private void EnableIcon(bool enable)
    {
        interactionIcon.SetActive(enable);
    }
}
