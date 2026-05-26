using UnityEngine;

public class Cage : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer targetRenderer;

    private Renderer rend;

    private bool _canBeHighlighted = true;

    private uint _defaultLayerMask = 1u << 0;
    private uint _outlineLayerMask = 1u << 8;

    [SerializeField] private GameObject lockCanvas;
    private bool isCanvasShown = false;

    public static Cage Instance;

    private void Awake()
    {
        Instance = this;

        rend = targetRenderer;
        lockCanvas.SetActive(false);
    }

    private void Update()
    {
        if (isCanvasShown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideCanvas();
            }
        }
    }
    public void Interact()
    {
        if (!isCanvasShown)
        {
            lockCanvas.SetActive(true);
            isCanvasShown = !isCanvasShown;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            InputManager.Instance.DisableAllControls();
        }
        else
        {
            lockCanvas.SetActive(false);
            isCanvasShown = !isCanvasShown;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputManager.Instance.EnablePlayerControls();
        }
        
    }
    public void HideCanvas()
    {
        lockCanvas.SetActive(false);
        isCanvasShown = !isCanvasShown;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.EnablePlayerControls();
    }

    public void Highlight(bool state)
    {
        if (!_canBeHighlighted) return;

        if (state)
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask | _outlineLayerMask;
        }
        else
        {
            targetRenderer.renderingLayerMask = _defaultLayerMask;
        }
    }
}
