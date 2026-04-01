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
    }

    public void ManageMovement()
    {
        _moveDir = _cameraObject.forward * InputManager.Instance.VerticalInput;
        _moveDir += _cameraObject.right * InputManager.Instance.HorizontalInput;

        _moveDir.y = 0;
        _moveDir.Normalize();

        Vector3 velocity = _moveDir * _moveSpeed;
        _playerRB.linearVelocity = velocity;
    }

    public void ManageRotation()
    {
        float mouseX = InputManager.Instance.LookX * _rotationSpeed * Time.deltaTime;
        float mouseY = InputManager.Instance.LookY * _rotationSpeed * Time.deltaTime;

        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        _cameraObject.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}