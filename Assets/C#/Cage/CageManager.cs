using UnityEngine;
using DG.Tweening;

public class CageManager : InteractableBase
{
    public static CageManager Instance { get; private set; }

    [SerializeField] private GameObject cage1Doors;
    [SerializeField] private GameObject cage2Doors;

    [SerializeField] private float rotateDuration = 0.5f;

    [SerializeField] private bool cage1ShouldBeOpen = true;
    [SerializeField] private bool cage2ShouldBeOpen = false;



    public bool Cage2ShouldBeOpen => cage2ShouldBeOpen;

    private bool _isItFirstHighlight = true;

    private void Awake()
    {
        base.Awake();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        ApplyState();
    }

    public override void Interact()
    {
        base.Interact();
        
        cage1ShouldBeOpen = !cage1ShouldBeOpen;
        cage2ShouldBeOpen = !cage2ShouldBeOpen;

        ApplyState();
    }

    public void OpenCage1()
    {
        cage1ShouldBeOpen = true;
        cage2ShouldBeOpen = false;

        ApplyState();
    }

    public void OpenCage2()
    {
        cage1ShouldBeOpen = false;
        cage2ShouldBeOpen = true;

        ApplyState();
    }

    private void ApplyState()
    {
        cage1Doors.transform.DOKill();
        cage2Doors.transform.DOKill();

        if (cage1ShouldBeOpen)
        {
            cage1Doors.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), rotateDuration);
            cage2Doors.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), rotateDuration);
        }
        else
        {
            cage1Doors.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), rotateDuration);
            cage2Doors.transform.DOLocalRotate(new Vector3(0f, 90f, 0f), rotateDuration);
        }
    }

    public override void Highlight(bool state)
    {
        if (_isItFirstHighlight)
        {
            _isItFirstHighlight = false;
            JournalManager.Instance.AddEntry("Cage");
        }
        base.Highlight(state);
    }
}