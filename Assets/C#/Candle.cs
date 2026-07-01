using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Candle : InteractableBase, IPickableObject
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;
    
    private bool _isPicked;

    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float lerpSpeed = 1.0f;
    
    [SerializeField] private ParticleSystem waxParticle;
    
    [SerializeField] private GameObject flames;
    
    private bool _canBeHighlighted = true;
    
    private bool hasSpilled;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;
    
    private void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Highlight(true);
    }

    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += OnSleepCandleChanges;
    }

    private void OnDestroy()
    {
        SleepManager.OnSleepStateChanged -= OnSleepCandleChanges;
    }

    private void OnSleepCandleChanges(bool isSleeping)
    {
        flames.SetActive(isSleeping);
    }

    public void OnPick(Transform point)
    {
        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;
        
        Highlight(false);
        _canBeHighlighted = false;



        float pickUpDuation = 1f;
        transform.DOMove(holdPoint.position, pickUpDuation);
        transform.SetParent(holdPoint);

    }

    public void OnDrop()
    {
        DOTween.KillAll();
        
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

        Vector3 camRight = camera.right;
        Vector3 camUp = camera.up;

        transform.Rotate(camRight, rotX, Space.World);
        transform.Rotate(camUp, rotY, Space.World);

        CheckRotationForSpillage();
    }

    public void CheckRotationForSpillage()
    {
        if (hasSpilled) return;

        float tilt = Vector3.Angle(transform.up, Vector3.up);

        if (tilt >= 60f)
        {
            hasSpilled = true;

            waxParticle.Play();
            InputManager.Instance.DisableAllControls();
            StartCoroutine(ShowDroppingWax());
            DoorManager.Instance.ShowNextObject();

            _canBeHighlighted = false;
        }
    }
    private IEnumerator ShowDroppingWax()
    {
        float droppingWaxDuration = 1.8f;
        
        yield return new WaitForSeconds(droppingWaxDuration);
        SleepManager.Instance.ChangeSleepStateWhiteFade();
        yield return new WaitForSeconds(ScreenFader.Instance.duration);
        DropAfterSolving();
        PlayerManager.Instance.CurrentObject = null;
        InputManager.Instance.EnablePlayerControls();
        PlayerLocomotion.Instance.CameraLocked = false;
        
        JournalManager.Instance.AddEntry("Candle");

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
    private void DropAfterSolving()
    {
        _isPicked = false;
        transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _canBeHighlighted = false;
    }

    public override void Interact()
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

    public override void Highlight(bool state)
    {
        if (!_canBeHighlighted)
            return;
        base.Highlight(state);
    }
}