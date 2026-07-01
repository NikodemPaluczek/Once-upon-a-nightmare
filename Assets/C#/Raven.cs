using UnityEngine;

public class Raven : InteractableBase
{
    public static Raven Instance;
    
    [SerializeField] private Transform pointA;
    [SerializeField] private Vector3 fixedRotation;

    [SerializeField] private Transform holdPoint;

    [SerializeField] private GameObject gobelinLight;

    [SerializeField] private float rayDistance = 3f;
    
    [SerializeField] private AudioSource ravenSound;

    private bool isHolding;
    private bool canPutBack;
    private bool canHighlight;

    private Camera mainCamera;
    private Collider objectCollider;
    private Transform currentPutBackTarget;

    private int KrukLayer = 8;


    [SerializeField] private GameObject lightVisualization;

    private void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        objectCollider = GetComponent<Collider>();
    }

    private void Update()
    {
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

    public void StopRockLight()
    {
        canHighlight = true;
        lightVisualization.SetActive(false);
        
        transform.position = pointA.position;

        if (gobelinLight != null)
            gobelinLight.SetActive(false);

        if (ravenSound != null)
        {
            ravenSound.Play();
        }
        
        JournalManager.Instance.AddEntry("Raven");
    }

    public override void Interact()
    {
        base.Interact();
        
        PickUp();
    }

    private void PickUp()
    {
        isHolding = true;

        if (objectCollider != null)
            objectCollider.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
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
                return;
            }
        }

        canPutBack = false;
        currentPutBackTarget = null;
    }

    private void PutBack()
    {
        PlayerManager.Instance.CurrentInteractable = null;
        
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
        lightVisualization.SetActive(true);
    }
}