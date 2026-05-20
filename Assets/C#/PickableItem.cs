using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickableObject, IInteractable
{
    private Rigidbody _rb;

    [SerializeField] private Transform camera;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private bool _isPicked;

    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float lerpSpeed = 1.0f;

    //[SerializeField] private GameObject waxClue;
    [SerializeField] private ParticleSystem waxParticle;

    //raczej placeholdy az beda 2 rodzaje swieczek
    [SerializeField] private GameObject flames;

    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Renderer targetRenderer;

    private Renderer rend;
    private Material[] original;

    private bool _canBeHighlighted = true;

    [SerializeField] private GameObject placeholdertext;


    private uint _defaultMask;
    private uint _outlineMask = 1u << 8;

    [SerializeField] private Transform holdPoint;
    [SerializeField] private Transform lookTarget;

    private void Start()
    {
        targetRenderer.renderingLayerMask = _defaultMask | _outlineMask;
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        rend = targetRenderer;
        original = rend.sharedMaterials;

        _defaultMask = targetRenderer.renderingLayerMask;
    }
    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += OnSleepCandleChanges;
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

        transform.SetParent(point);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _canBeHighlighted = false;
        Highlight(false);
        StartCoroutine(LerpBetweenPoints(holdPoint));
    }

    public void OnDrop()
    {
        _isPicked = false;

        transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;

        _canBeHighlighted = true;
    }

    private IEnumerator LerpBetweenPoints(Transform holdPoint)
    {
        transform.localRotation = Quaternion.identity;

        float time = 0f;
        float duration = 1f / lerpSpeed;

        while (time < duration)
        {
            float t = time / duration;

            transform.position = Vector3.Lerp(_startPosition, holdPoint.position, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = holdPoint.position;
        transform.SetParent(holdPoint);
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
    private bool hasTriggered;

    public void CheckRotationForSpillage()
    {
        if (hasTriggered) return;

        float tilt = Vector3.Angle(transform.up, Vector3.up);

        if (tilt >= 60f)
        {
            hasTriggered = true;

            waxParticle.Play();
            InputManager.Instance.DisableAllControls();
            StartCoroutine(WaitforSeconds());
            DoorManager.Instance.ShowNextObject();

            _canBeHighlighted = false;
        }
    }
    private IEnumerator WaitforSeconds()
    {
        yield return new WaitForSeconds(1.8f); //we wait 1 sec cuz we want to show player dripping wax
        SleepManager.Instance.ChangeSleepStateWhiteFade();
        yield return new WaitForSeconds(1); //fade out duration
        DropAfterSolving();
        PlayerManager.Instance.CurrentObject = null;
        InputManager.Instance.EnablePlayerControls();
        PlayerLocomotion.Instance.CameraLocked = false;

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
        placeholdertext.SetActive(true);
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

    public void Highlight(bool state)
    {
        if (state)
        {
            targetRenderer.renderingLayerMask = _defaultMask | _outlineMask;
        }
        else
        {
            targetRenderer.renderingLayerMask = _defaultMask;
        }
    }
}