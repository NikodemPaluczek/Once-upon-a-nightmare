using UnityEngine;

public class Kruk : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Vector3 fixedRotation;

    [SerializeField] private Transform holdPoint;

    [SerializeField] private GameObject gobelinLight;

    [SerializeField] private float rayDistance = 3f;

    [SerializeField] private Renderer targetRenderer;

    private bool isHolding;
    private bool canPutBack;
    private bool canHighlight;

    private Camera mainCamera;
    private Collider objectCollider;
    private Transform currentPutBackTarget;

    private int KrukLayer = 8;

    private uint _defaultMask;
    private uint _outlineMask = 1u << 8;

    private void Awake()
    {
        _defaultMask = targetRenderer.renderingLayerMask;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TeleportToPointA();
            canHighlight = true;
        }

        if (isHolding)
        {
            canHighlight = false;

            CheckPutBackRaycast();

            if (canPutBack && Input.GetKeyDown(KeyCode.E))
            {
                PutBack();
            }
        }
    }

    private void TeleportToPointA()
    {
        transform.position = pointA.position;
        transform.rotation = Quaternion.Euler(fixedRotation);

        if (gobelinLight != null)
            gobelinLight.SetActive(false);
    }

    public void Interact()
    {
        PickUp();
    }

    public void Highlight(bool state)
    {
        if (!canHighlight || isHolding)
        {
            targetRenderer.renderingLayerMask = _defaultMask;
            return;
        }

        if (state)
        {
            targetRenderer.renderingLayerMask = _defaultMask | _outlineMask;
        }
        else
        {
            targetRenderer.renderingLayerMask = _defaultMask;
        }
    }

    private void PickUp()
    {
        isHolding = true;

        if (objectCollider != null)
            objectCollider.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void CheckPutBackRaycast()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.gameObject.layer == KrukLayer)
            {
                canPutBack = true;
                currentPutBackTarget = hit.collider.transform;

                Debug.Log("mo¿na od³o¿yæ kruk");
                return;
            }

            Debug.Log("Trafiono: " + hit.collider.name + " | layer: " + hit.collider.gameObject.layer);
        }

        canPutBack = false;
        currentPutBackTarget = null;
    }

    private void PutBack()
    {
        if (currentPutBackTarget == null)
            return;

        isHolding = false;

        transform.SetParent(currentPutBackTarget);
        transform.position = currentPutBackTarget.position;
        transform.rotation = currentPutBackTarget.rotation;

        if (objectCollider != null)
            objectCollider.enabled = true;

        if (gobelinLight != null)
            gobelinLight.SetActive(true);
    }
}