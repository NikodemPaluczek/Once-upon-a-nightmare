using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public InputSystem_Actions inputActions;

    // Movement
    private Vector2 movementInput;
    public float VerticalInput;
    public float HorizontalInput;

    // Look
    private Vector2 lookInput;
    public float LookX;
    public float LookY;

    // Rotate object
    private Vector2 rotateInput;
    public Vector2 RotateInput;

    // Interact
    public bool InteractPressed;

    public bool IsInObjectMode;


    private void Start()
    {
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();

            // player map
            inputActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += i => movementInput = Vector2.zero;

            inputActions.Player.Look.performed += i => lookInput = i.ReadValue<Vector2>();
            inputActions.Player.Look.canceled += i => lookInput = Vector2.zero;

            // object map
            inputActions.PickableObject.Rotate.performed += i => rotateInput = i.ReadValue<Vector2>();
            inputActions.PickableObject.Rotate.canceled += i => rotateInput = Vector2.zero;

            // interaction map
            inputActions.Interaction.Interact.performed += i => InteractPressed = true;
        }

        inputActions.Interaction.Enable();
        EnablePlayerControls();
    }

    private void Update()
    {
        VerticalInput = movementInput.y;
        HorizontalInput = movementInput.x;

        LookX = lookInput.x;
        LookY = lookInput.y;

        RotateInput = rotateInput;
    }

    public void EnablePlayerControls()
    {
        IsInObjectMode = false;

        inputActions.PickableObject.Disable();
        inputActions.Player.Enable();
        inputActions.Interaction.Enable();
    }

    public void EnableObjectControls()
    {
        IsInObjectMode = true;

        inputActions.Player.Disable();
        inputActions.PickableObject.Enable();
    }
    public void DisableAllControls()
    {
        IsInObjectMode = false;

        inputActions.Player.Disable();
        inputActions.PickableObject.Disable();
        inputActions.Interaction.Disable();

        movementInput = Vector2.zero;
        lookInput = Vector2.zero;
        rotateInput = Vector2.zero;

        VerticalInput = 0;
        HorizontalInput = 0;
        LookX = 0;
        LookY = 0;

        InteractPressed = false;
    }
}