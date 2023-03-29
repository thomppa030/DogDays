using UnityEngine;

public class InteractionCheckRayCast : MonoBehaviour
{
    [Header("InteractionInRange")]
    [SerializeField] float rayDistance = 1f;
    [Header("Icons")]
    [SerializeField] private GameObject interactionIcon;
    
    public RaycastHit hit;
    
    private PlayerStateMachine _playerStateMachine;

    private Camera cam;

    private void Start()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        
        EnableIcon(false);
        cam = Camera.main;
    }

    private void Update()
    {
        CheckForInteraction();

        if (interactionIcon.activeSelf) interactionIcon.transform.LookAt(cam.transform);
    }

    void CheckForInteraction()
    {
        if (InteractionInRange())
        {
            EnableIcon(true);
        }
        else
        {
            EnableIcon(false);
        }
    }

    public bool InteractionInRange()
    {
        //ObjectLayers
        int layerMask = 1 << 6 ^ 1 << 10;
        

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }       
    }


    private void EnableIcon(bool enable)
    {
        interactionIcon.SetActive(enable);
    }
}
