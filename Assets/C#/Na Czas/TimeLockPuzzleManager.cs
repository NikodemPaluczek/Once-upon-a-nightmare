using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimedLockPuzzleManager : MonoBehaviour
{
    public static TimedLockPuzzleManager Instance;

    [Header("Code")]
    [SerializeField] private string correctCode = "2103";

    [Header("Buttons")]
    [SerializeField] private DigitButton[] digitButtons;

    [Header("Timer")]
    [SerializeField] private float duration = 60f;

    [SerializeField] private TMP_Text timerText;

    private float currentTime;

    private bool puzzleActive;

    [SerializeField] private GameObject cage;

    [SerializeField] private GameObject timer;

    private void Awake()
    {
        Instance = this;
        cage.SetActive(false);
        timer.SetActive(false);
    }

    public void StartPuzzle()
    {
        currentTime = duration;

        puzzleActive = true;

        cage.SetActive (true);
        timer.SetActive (true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            StartPuzzle();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (!puzzleActive)
            return;

        currentTime -= Time.deltaTime;

        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            currentTime = 0f;

            FailPuzzle();
        }
        
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CheckCode()
    {
        if (!puzzleActive)
            return;

        string enteredCode = "";

        for (int i = 0; i < digitButtons.Length; i++)
        {
            enteredCode += digitButtons[i].CurrentValue.ToString();
        }

        Debug.Log("CURRENT CODE: " + enteredCode);

        if (enteredCode == correctCode)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleActive = false;

        cage.SetActive(false);

        GridFireplaceManager.Instance.SetInteractionsEnabled(false);

        Cage.Instance.HideCanvas();
        timer.SetActive(false);
        Debug.Log("PUZZLE COMPLETE");
    }

    private void FailPuzzle()
    {
        puzzleActive = false;

        Debug.Log("GAME OVER");

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}