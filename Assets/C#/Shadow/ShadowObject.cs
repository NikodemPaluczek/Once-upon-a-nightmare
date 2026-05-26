using UnityEngine;

public class ShadowObject : MonoBehaviour, IPickableObject, IInteractable
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;

    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private Vector3 targetEuler;
    [SerializeField] private float tolerance = 10f;

    [SerializeField] private float solveHoldTime = 2f;

    private float _solveTimer;

    private bool _isPicked;
    private bool _canBeHighlighted = true;
    private bool _solved;

    private uint _defaultLayerMask = 1u << 0;
    private uint _outlineLayerMask = 1u << 8;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _defaultLayerMask = targetRenderer.renderingLayerMask;
    }

    public void Interact()
    {
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
        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;

        transform.SetParent(point);
        transform.localPosition = Vector3.zero;

        _canBeHighlighted = false;
        Highlight(false);
    }

    public void OnDrop()
    {
        _isPicked = false;

        transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;

        _canBeHighlighted = true;

        _solveTimer = 0f;
    }

    public void Rotate(Vector2 input)
    {
        if (!_isPicked || _solved) return;

        float rotX = input.y * rotationSpeed * Time.deltaTime;
        float rotY = -input.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(camera.right, rotX, Space.World);
        transform.Rotate(camera.up, rotY, Space.World);

        CheckSolution();
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

    private void CheckSolution()
    {
        Vector3 current = transform.localEulerAngles;

        float dx = Mathf.Abs(Mathf.DeltaAngle(current.x, targetEuler.x));
        float dy = Mathf.Abs(Mathf.DeltaAngle(current.y, targetEuler.y));
        float dz = Mathf.Abs(Mathf.DeltaAngle(current.z, targetEuler.z));

        bool withinTolerance =
            dx <= tolerance &&
            dy <= tolerance &&
            dz <= tolerance;

        if (withinTolerance)
        {
            _solveTimer += Time.deltaTime;

            if (_solveTimer >= solveHoldTime)
            {
                _solved = true;
                Solve();
            }
        }
        else
        {
            _solveTimer = 0f;
        }
    }

    private void Solve()
    {
        OnDrop();

        PlayerManager.Instance.CurrentObject = null;

        InputManager.Instance.EnablePlayerControls();

        PlayerLocomotion.Instance.CameraLocked = false;

        _canBeHighlighted = false;
        targetRenderer.renderingLayerMask = _defaultLayerMask;

        DoorManager.Instance.ShowNextObject();
    }

    public void Highlight(bool state)
    {
        if (!_canBeHighlighted) return;

        if (state)
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask | _outlineLayerMask;
        }
        else
        {
            targetRenderer.renderingLayerMask = _defaultLayerMask;
        }
    }
}