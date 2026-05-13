using TMPro;
using UnityEngine;

public class DigitButton : MonoBehaviour
{
    [SerializeField] private TMP_Text digitText;

    public int CurrentValue { get; private set; }

    private void Start()
    {
        UpdateVisual();
    }

    public void IncreaseDigit()
    {
        CurrentValue++;

        if (CurrentValue > 9)
            CurrentValue = 0;

        UpdateVisual();

        TimedLockPuzzleManager.Instance.CheckCode();
    }

    private void UpdateVisual()
    {
        digitText.text = CurrentValue.ToString();
    }
}