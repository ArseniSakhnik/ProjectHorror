using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject InteractIcon;
    public GameObject Dot;
    [Header("Raycast")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = 1f;
    [SerializeField] private LayerMask interactionLayer = default;
    [SerializeField] private Interactable currentInteractable;
    [SerializeField] private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }


    private void HandleInterationCheck()
    {

        if (currentInteractable != null)
        {
            currentInteractable.OnLoseFocus();
            InteractIcon.SetActive(false);
            Dot.SetActive(true);
        }

        if (Physics.Raycast(cam.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {

                hit.collider.TryGetComponent(out currentInteractable);


                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                    InteractIcon.SetActive(true);
                    Dot.SetActive(false);
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
            InteractIcon.SetActive(false);
            Dot.SetActive(true);
        }

    }

    private void HandleInterationInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable !=null && Physics.Raycast(cam.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
            InteractIcon.SetActive(false);
            Dot.SetActive(false);
        }
    }

   


    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.isblockInteraction)
        {
            HandleInterationCheck();
            HandleInterationInput();
        }
    }
}
