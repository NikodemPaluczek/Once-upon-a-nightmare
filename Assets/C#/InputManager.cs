using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public InputSystem_Actions inputActions;

    //Movement
    private Vector2 movementInput;

    public float VerticalInput;
    public float HorizontalInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
    private void OnEnable()
    {

        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.Move.performed +=
                i => movementInput = i.ReadValue<Vector2>();
            inputActions.Player.Move.canceled +=
                i => movementInput = Vector2.zero;

        }
        inputActions.Enable();
    }
    private void Update()
    {
        HandleMovementInput();
    }

    public void HandleMovementInput()
    {
        VerticalInput = movementInput.y;
        HorizontalInput = movementInput.x;
    }
}
