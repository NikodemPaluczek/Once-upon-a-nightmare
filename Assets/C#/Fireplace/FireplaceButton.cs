using UnityEngine;
using UnityEngine.UI;

public class FireplaceButton : InteractableBase
{
    private Image img;

    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color blockedColor = Color.gray;

    public bool IsSelected { get; private set; }
    public bool IsBlockedExternally { get; private set; }

    private bool isHighlighted;

    private int row;
    private int col;
    private int index;


    private void Awake()
    {
        img = GetComponent<Image>();
        img.color = defaultColor;
    }

    public void SetCoordinates(int r, int c, int i)
    {
        row = r;
        col = c;
        index = i;
    }

    public override void Interact()
    {
        if (IsBlockedExternally) return;

        GridFireplaceManager.Instance.ToggleFireplace(row, col, index);
    }

    public override void Highlight(bool state)
    {
        if (GridFireplaceManager.Instance.isItFirstHighlight)
        {
            GridFireplaceManager.Instance.isItFirstHighlight = false;
            JournalManager.Instance.AddEntry("FirePlace");
        }

        if (IsBlockedExternally || IsSelected) return;

        isHighlighted = state;
        UpdateColor();
    }

    public void SetSelected(bool value)
    {
        IsSelected = value;
        UpdateColor();
    }

    public void SetBlocked(bool value)
    {
        IsBlockedExternally = value;
        UpdateColor();
    }

    public void ForceReset()
    {
        IsSelected = false;
        IsBlockedExternally = false;
        isHighlighted = false;
        img.color = defaultColor;
    }

    private void UpdateColor()
    {
        if (IsBlockedExternally)
        {
            img.color = blockedColor;
            return;
        }

        if (IsSelected)
        {
            img.color = selectedColor;
            return;
        }

        if (isHighlighted)
        {
            img.color = highlightColor;
            return;
        }

        img.color = defaultColor;
    }
}