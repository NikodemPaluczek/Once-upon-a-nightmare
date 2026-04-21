using UnityEngine;
using UnityEngine.UI;

public class FireplaceButton : MonoBehaviour
{
    private Image fireplaceImage;
    [SerializeField] private Color highlightColor;
    private void OnEnable()
    {
        fireplaceImage = GetComponent<Image>();
    }
    public void HighlightColorChange()
    {
        fireplaceImage.color = highlightColor;
    }
    public void ResetColor()
    {
        fireplaceImage.color = Color.white;
    }
}
