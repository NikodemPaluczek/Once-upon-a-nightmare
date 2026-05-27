using UnityEngine;

public class CageManager : MonoBehaviour, IInteractable
{
    [Header("Cages")]
    [SerializeField] private GameObject cage1Doors;
    [SerializeField] private GameObject cage2Doors;

    [Header("State")]
    [SerializeField] private bool cage1ShouldBeOpen = true;
    [SerializeField] private bool cage2ShouldBeOpen = false;

    [Header("Outline")]
    [SerializeField] private Renderer targetRenderer;

    private uint _defaultMask;
    private uint _outlineMask = 1u << 8;

    private void Awake()
    {
        _defaultMask = targetRenderer.renderingLayerMask;
    }

    private void Start()
    {
        ApplyState();
    }

    public void Interact()
    {
        // Zamiana stanów
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
        // true = klatka otwarta -> drzwi wy³¹czone
        cage1Doors.SetActive(!cage1ShouldBeOpen);
        cage2Doors.SetActive(!cage2ShouldBeOpen);
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