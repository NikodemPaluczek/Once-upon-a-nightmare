using UnityEngine;

public class KeyStatusManager : MonoBehaviour
{
    public static KeyStatusManager Instance;

    public bool isCorrectPicked;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateKeyStatus(bool isWrong)
    {
        isCorrectPicked = !isWrong;
    }
}