using UnityEngine;
using static UnityEngine.UI.Image;

public class Wrzeciono : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer targetRenderer;

    private Renderer rend;

    private bool _canBeHighlighted = true;

    private uint _defaultLayerMask = 1u << 0;
    private uint _outlineLayerMask = 1u << 8;

    private void Awake()
    {
        rend = targetRenderer;
    }


    public void Interact()
    {
        SleepManager.Instance.ChangeSleepState();
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
