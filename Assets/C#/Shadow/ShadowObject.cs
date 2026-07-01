using System.Collections;
using UnityEngine;

public class ShadowObject : InteractableBase, IPickableObject
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;

    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private Vector3 targetEuler;
    [SerializeField] private float tolerance = 10f;

    [SerializeField] private float solveHoldTime = 2f;

    [SerializeField] private AudioSource solveAudioSource;

    [SerializeField] private GameObject topestryNew;
    [SerializeField] private GameObject topestryOld;
    [SerializeField] private GameObject topestryFire;
    
    private bool _isPickedFirstTime = true;
    
    private float _solveTimer;
    private bool _isPicked;
    private bool _solved;


    private void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }
    
    public override void Interact()
    {
        base.Interact();
        
        if (!_isPicked)
        {
            OnPick(holdPoint);
            PlayerManager.Instance.CurrentObject = this;
            InputManager.Instance.EnableObjectControls();
            PlayerLocomotion.Instance.CameraLocked = true;
        }

        if (_isPickedFirstTime)
        {
            _isPickedFirstTime = false;
            JournalManager.Instance.AddEntry("Rock");
            StartCoroutine(InteruptWithRaven());
        }
    }

    public override void CancelInteraction()
    {
        base.CancelInteraction();
        
        OnDrop();
        PlayerManager.Instance.CurrentObject = null;
        InputManager.Instance.EnablePlayerControls();
        PlayerLocomotion.Instance.CameraLocked = false;
    }

    public IEnumerator InteruptWithRaven()
    {
        float ravenWaitTime = 2f;
        yield return new WaitForSeconds(ravenWaitTime);
        Raven.Instance.StopRockLight();
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
        if (!_isPicked || _solved)
            return;

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

        _canBeHighlighted = true;
        Highlight(false);
        _canBeHighlighted = false;


        if (solveAudioSource != null)
        {
            solveAudioSource.Play();
        }

        topestryNew.transform.localPosition += new Vector3(100f, 0f, 0f);
        topestryOld.transform.localPosition += new Vector3(100f, 0f, 0f);
        topestryFire.SetActive(true);

        DoorManager.Instance.ShowNextObject();
    }
}