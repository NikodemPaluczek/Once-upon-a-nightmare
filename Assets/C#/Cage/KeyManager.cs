using UnityEngine;

public class KeyManager : MonoBehaviour, IPickableObject, IInteractable
{
    private Rigidbody _rb;

    [Header("References")]
    [SerializeField] private Transform camera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;

    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private GameObject interactionCanvas;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 150f;

    [Header("Key State")]
    [SerializeField] private bool isWrong;

    private bool _isPicked;
    private bool _canBeHighlighted = true;

    private uint _defaultLayerMask;
    private uint _outlineLayerMask = 1u << 8;

    public static bool KeyAlreadyTaken;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _defaultLayerMask = targetRenderer.renderingLayerMask;
    }

    private void Update()
    {
        if (!_isPicked) return;


        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeTheKey();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnDrop();

            PlayerManager.Instance.CurrentObject = null;

            InputManager.Instance.EnablePlayerControls();

            PlayerLocomotion.Instance.CameraLocked = false;
        }
    }

    private void LateUpdate()
    {
        if (_isPicked && lookTarget != null)
        {
            Vector3 dir = lookTarget.position - camera.position;

            Quaternion rot = Quaternion.LookRotation(dir);

            camera.rotation = Quaternion.Lerp(
                camera.rotation,
                rot,
                Time.deltaTime * 10f
            );
        }
    }

    public void Interact()
    {
        if (KeyAlreadyTaken) return;

        if (_isPicked)
        {
            OnDrop();

            PlayerManager.Instance.CurrentObject = null;

            InputManager.Instance.EnablePlayerControls();

            PlayerLocomotion.Instance.CameraLocked = false;
        }
        else
        {
            OnPick(holdPoint);

            PlayerManager.Instance.CurrentObject = this;

            InputManager.Instance.EnableObjectControls();

            PlayerLocomotion.Instance.CameraLocked = true;
        }
    }

    public void OnPick(Transform point)
    {
        if (KeyAlreadyTaken) return;

        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;

        transform.SetParent(point);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _canBeHighlighted = false;

        Highlight(false);

        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(true);
        }
    }

    public void OnDrop()
    {
        _isPicked = false;

        transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;

        _canBeHighlighted = true;

        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);
        }
    }

    public void Rotate(Vector2 input)
    {
        if (!_isPicked) return;

        float rotX = input.y * rotationSpeed * Time.deltaTime;
        float rotY = -input.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(camera.right, rotX, Space.World);
        transform.Rotate(camera.up, rotY, Space.World);
    }

    public void TakeTheKey()
    {
        if (KeyAlreadyTaken) return;

        KeyAlreadyTaken = true;

        KeyStatus.Instance.UpdateKeyStatus(isWrong);

        if (_isPicked)
        {
            PlayerManager.Instance.CurrentObject = null;

            InputManager.Instance.EnablePlayerControls();

            PlayerLocomotion.Instance.CameraLocked = false;
        }

        _canBeHighlighted = false;

        Highlight(false);

        if (interactionCanvas != null)
        {
            interactionCanvas.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void Highlight(bool state)
    {
        if (!_canBeHighlighted) return;

        if (KeyAlreadyTaken) return;

        if (state)
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask | _outlineLayerMask;
        }
        else
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask;
        }
    }
}