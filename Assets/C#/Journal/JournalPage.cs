using TMPro;
using UnityEngine;

public class JournalPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private bool isOccupied;

    public bool IsOccupied => isOccupied;

    public void SetEntry(string content)
    {
        if (textField == null) return;

        textField.text = content;
        isOccupied = true;
    }

    public void Clear()
    {
        if (textField == null) return;

        textField.text = "";
        isOccupied = false;
    }
}