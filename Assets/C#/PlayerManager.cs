using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private IInteractable currentInteractable;


    [SerializeField] private Transform holdPoint;
    [SerializeField] private float interactDistance = 3f;


    public IPickableObject CurrentObject;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        PlayerLocomotion.Instance.ManageRotation();
        CheckRayForHighlight();
        HandleInteraction();
        HandleObjectRotation();
        
    }

    private void CheckRayForHighlight()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            FireplaceButton fireplaceButton = hit.collider.GetComponent<FireplaceButton>();

            if (interactable != currentInteractable)
            {
                if (currentInteractable != null)
                    currentInteractable.Highlight(false);

                currentInteractable = interactable;

                if (currentInteractable != null)
                    currentInteractable.Highlight(true);
            }
                return;
        }

        if (currentInteractable != null)
        {
            currentInteractable.Highlight(false);
            currentInteractable = null;
        }
    }

    private void FixedUpdate()
    {
        if (!InputManager.Instance.IsInObjectMode)
        {
            PlayerLocomotion.Instance.ManageMovement();
        }
    }

    private void HandleInteraction()
    {
        if (!InputManager.Instance.InteractPressed)
            return;

        InputManager.Instance.InteractPressed = false;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            interactable?.Interact();
        }
        
        // if we r holding smth then we drop
        //if (CurrentObject != null)
        //{
        //    CurrentObject.OnDrop();
        //    CurrentObject = null;
        //
        //    InputManager.Instance.EnablePlayerControls();
        //    return;
        //}
        //
        //// if not then we check if we can pick up
        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        //
        //if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        //{
        //    IPickableObject pickable = hit.collider.GetComponent<IPickableObject>();
        //
        //    if (pickable != null)
        //    {
        //        CurrentObject = pickable;
        //        CurrentObject.OnPick(holdPoint);
        //
        //        InputManager.Instance.EnableObjectControls();
        //    }
        //}
    }

    private void HandleObjectRotation()
    {
        if (CurrentObject == null) return;

        CurrentObject.Rotate(InputManager.Instance.RotateInput);
    }
}