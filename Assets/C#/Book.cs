using UnityEngine;

public class Book : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private GameObject canvasGameObject;

    private bool _shouldShow = true;

    private uint _defaultMask;
    private uint _outlineMask = 1u << 8;

    private void Awake()
    {
        _defaultMask = targetRenderer.renderingLayerMask;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBook();
        }
    }

    public void Interact()
    {
        ToggleBook();
    }

    private void ToggleBook()
    {
        if (_shouldShow)
        {
            InputManager.Instance.DisableAllControls();
        }
        else
        {
            InputManager.Instance.EnablePlayerControls();
        }

        canvasGameObject.SetActive(_shouldShow);
        _shouldShow = !_shouldShow;
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