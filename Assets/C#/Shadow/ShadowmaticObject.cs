using UnityEngine;

public class ShadowmaticObject : MonoBehaviour, IPickableObject, IInteractable
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;

    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Material highlightMaterial;

    private Material[] originalMaterials;

    private bool _isPicked;
    private bool _canBeHighlighted = true;

    [SerializeField] private float rotationSpeed = 150f;

    [SerializeField] private Vector3 targetEuler;
    [SerializeField] private float tolerance = 10f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        originalMaterials = targetRenderer.sharedMaterials;
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
        transform.localRotation = Quaternion.identity;

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
    }

    public void Rotate(Vector2 input)
    {
        if (!_isPicked) return;

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
            camera.rotation = Quaternion.Lerp(camera.rotation, rot, Time.deltaTime * 10f);
        }
    }

    private void CheckSolution()
    {
        Vector3 current = transform.localEulerAngles;

        float dx = Mathf.Abs(Mathf.DeltaAngle(current.x, targetEuler.x));
        float dy = Mathf.Abs(Mathf.DeltaAngle(current.y, targetEuler.y));
        float dz = Mathf.Abs(Mathf.DeltaAngle(current.z, targetEuler.z));

        if (dx <= tolerance && dy <= tolerance && dz <= tolerance)
        {
            Solve();
        }
    }

    private void Solve()
    {
        OnDrop();

        PlayerManager.Instance.CurrentObject = null;
        InputManager.Instance.EnablePlayerControls();
        PlayerLocomotion.Instance.CameraLocked = false;
    }

    public void Highlight(bool state)
    {
        if (state && _canBeHighlighted)
        {
            Material[] mats = new Material[targetRenderer.sharedMaterials.Length];

            for (int i = 0; i < mats.Length; i++)
                mats[i] = highlightMaterial;

            targetRenderer.materials = mats;
        }
        else
        {
            targetRenderer.materials = originalMaterials;
        }
    }
}