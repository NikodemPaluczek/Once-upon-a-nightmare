using UnityEngine;

public class KeyStatus : MonoBehaviour
{
    public static KeyStatus Instance;

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