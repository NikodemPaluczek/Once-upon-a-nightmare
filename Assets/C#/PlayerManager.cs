using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private float interactDistance = 3f;

    private PickableObject currentObject;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        PlayerLocomotion.Instance.ManageRotation();

        HandleInteraction();
        HandleObjectRotation();
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

        // if we r holding smth then we drop
        if (currentObject != null)
        {
            currentObject.OnDrop();
            currentObject = null;

            InputManager.Instance.EnablePlayerControls();
            return;
        }

        // if not then we check if we can pick up
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            PickableObject pickable = hit.collider.GetComponent<PickableObject>();

            if (pickable != null)
            {
                currentObject = pickable;
                currentObject.OnPick(holdPoint);

                InputManager.Instance.EnableObjectControls();
            }
        }
    }

    private void HandleObjectRotation()
    {
        if (currentObject == null) return;

        currentObject.Rotate(InputManager.Instance.RotateInput);
    }
}