using UnityEngine;
using UnityEngine.UI;

public class FireplaceButton : MonoBehaviour, IInteractable
{
    private Image fireplaceImage;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;

    private bool _isSelected = false;
    private void OnEnable()
    {
        fireplaceImage = GetComponent<Image>();
    }
    public void Interact()
    {
        _isSelected = !_isSelected;
        fireplaceImage.color = _isSelected ? selectedColor : highlightColor;

    }

    public void Highlight(bool state)
    {
        if (_isSelected)
            return;
        fireplaceImage.color = state ? highlightColor : defaultColor;
    }
}
