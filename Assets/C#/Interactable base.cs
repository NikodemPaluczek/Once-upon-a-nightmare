using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    private Renderer[] _targetRenderers;

    private readonly uint _defaultMask = 1u << 0;
    private readonly uint _outlineMask = 1u << 8;

    protected bool _canBeHighlighted = true;
    protected virtual void Awake()
    {
        _targetRenderers = GetComponentsInChildren<Renderer>();
    }

    public virtual void Highlight(bool state)
    {
        if (!_canBeHighlighted)
            return;
        
        if (_targetRenderers == null || _targetRenderers.Length == 0)
            return;
        
        
        foreach (Renderer renderer in _targetRenderers)
        {
            renderer.renderingLayerMask = state?
                (_defaultMask | _outlineMask):
                _defaultMask;
        }
    }

    public virtual void Interact()
    {
        PlayerManager.Instance.CurrentInteractable = this;
    }

    public virtual void CancelInteraction()
    {
        PlayerManager.Instance.CurrentInteractable = null;
    }
}