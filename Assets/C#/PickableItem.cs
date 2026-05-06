using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickableObject, IHighlightable
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
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        rend = targetRenderer;
        original = rend.sharedMaterials;
    }
    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += OnSleepCandleChanges;
    }
    private void OnSleepCandleChanges(bool isSleeping)
    {
        flames.SetActive(isSleeping);
    }

    public void OnPick(Transform holdPoint)
    {
        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;

        _canBeHighlighted = false;
        SetHighlight(false);
        StartCoroutine(LerpBetweenPoints(holdPoint));
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
    public void OnDrop()
    {
        _isPicked = false;

        transform.SetParent(null);

        transform.position = _startPosition;
        transform.rotation = _startRotation;

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
        }
    }
    private IEnumerator WaitforSeconds()
    {
        yield return new WaitForSeconds(1.8f); //we wait 1 sec cuz we want to show player dripping wax
        SleepManager.Instance.ChangeSleepState();
        yield return new WaitForSeconds(1); //fade out duration
        DropAfterSolving();
        PlayerManager.Instance.CurrentObject = null;

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




    public void SetHighlight(bool state)
    {
        if (state && _canBeHighlighted)
        {
            var mats = rend.sharedMaterials;

            Material[] newMats = new Material[mats.Length];

            for (int i = 0; i < newMats.Length; i++)
                newMats[i] = highlightMaterial;

            rend.materials = newMats;
        }
        else
        {
            rend.materials = original;
        }
    }
}