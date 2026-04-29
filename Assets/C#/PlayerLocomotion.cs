using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    public static PlayerLocomotion Instance;

    [SerializeField] private Transform _cameraObject;

    private Rigidbody _playerRB;

    private Vector3 _moveDir;

    private float _moveSpeed = 4f;
    private float _rotationSpeed = 200f;

    private float _yRotation;
    private float _xRotation;

    private Vector3 _cameraDefaultLocalPos;

    public bool CameraLocked;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
        _cameraDefaultLocalPos = _cameraObject.localPosition;
    }

    public void ManageMovement()
    {
        _moveDir = _cameraObject.forward * InputManager.Instance.VerticalInput;
        _moveDir += _cameraObject.right * InputManager.Instance.HorizontalInput;

        _moveDir.y = 0;
        _moveDir.Normalize();

        _playerRB.linearVelocity = _moveDir * _moveSpeed;
    }

    public void ManageRotation()
    {
        if (CameraLocked) return;

        float mouseX = InputManager.Instance.LookX * _rotationSpeed * Time.deltaTime;
        float mouseY = InputManager.Instance.LookY * _rotationSpeed * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _cameraObject.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    public void ManageCrouch()
    {
        Vector3 targetPos = _cameraDefaultLocalPos;

        if (InputManager.Instance.CrouchHeld)
            targetPos += new Vector3(0f, -1f, 0f);

        _cameraObject.localPosition = Vector3.Lerp(
            _cameraObject.localPosition,
            targetPos,
            Time.deltaTime * 6f
        );
    }
}