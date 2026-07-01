using DG.Tweening;
using UnityEngine;

public class Key : InteractableBase, IPickableObject
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;


    [SerializeField] private GameObject interactionCanvas;

    [SerializeField] private float rotationSpeed = 150f;

    [SerializeField] private bool isWrong;

    private bool _isPicked;


    public static bool KeyAlreadyTaken;

    private void Awake()
    {
        base.Awake();
        
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isPicked)
            return;

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

    public override void Interact()
    { 
        if (CageManager.Instance != null && !CageManager.Instance.Cage2ShouldBeOpen)
            return;

        if (KeyAlreadyTaken)
            return;

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
        
        base.Interact();
    }

    public void OnPick(Transform point)
    {
        if (KeyAlreadyTaken)
            return;

        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;

        transform.SetParent(point);

        transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            transform.DOLocalMoveY(
                transform.localPosition.y + 1f,
                0.5f
            )
        );

        sequence.Append(
            transform.DOLocalMove(
                Vector3.zero,
                0.5f
            )
        );

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

        transform.DOKill();
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
        if (!_isPicked)
            return;

        float rotX = input.y * rotationSpeed * Time.deltaTime;
        float rotY = -input.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(camera.right, rotX, Space.World);
        transform.Rotate(camera.up, rotY, Space.World);
    }

    public void TakeTheKey()
    {
        if (KeyAlreadyTaken)
            return;

        KeyAlreadyTaken = true;

        KeyStatusManager.Instance.UpdateKeyStatus(isWrong);

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

    public override void Highlight(bool state)
    {
        if (CageManager.Instance != null && !CageManager.Instance.Cage2ShouldBeOpen)
        {
            return;
        }
        if (KeyAlreadyTaken)
            return;
        
        base.Highlight(state);
    }
}