using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TimedLockPuzzleManager : MonoBehaviour
{
    public static TimedLockPuzzleManager Instance;

    [SerializeField] private string correctCode = "2103";
    [SerializeField] private DigitButton[] digitButtons;

    [SerializeField] private ChestOpener chestPrefab;
    [SerializeField] private Transform chestSpawnPoint;

    [SerializeField] private GameObject splinde;
    [SerializeField] private Transform splindeTargetPoint;

    [SerializeField] private GameObject lockCanvas;

    [SerializeField] private AudioSource completeAudioSource;
    [SerializeField] private AudioSource failAudioSource;

    private bool puzzleActive;
    private bool isCanvasShown;
    private ChestOpener spawnedChest;

    private void Awake()
    {
        Instance = this;
        lockCanvas.SetActive(false);
    }

    public void StartPuzzle()
    {
        JournalManager.Instance.AddEntry("Timelock");
        
        puzzleActive = true;

        if (spawnedChest == null)
        {
            spawnedChest = Instantiate(
                chestPrefab,
                chestSpawnPoint.position,
                chestSpawnPoint.rotation
            );

            splinde.transform.position = splindeTargetPoint.position;
            splinde.transform.rotation = splindeTargetPoint.rotation;
        }
        
    }

    private void Update()
    {
        if (isCanvasShown && Input.GetKeyDown(KeyCode.Escape))
        {
            HideLockCanvas();
        }

        if (!puzzleActive)
            return;
        
    }


    public void ShowLockCanvas()
    {
        if (isCanvasShown)
            return;

        isCanvasShown = true;
        lockCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        InputManager.Instance.DisableAllControls();
    }

    public void HideLockCanvas()
    {
        if (!isCanvasShown)
            return;

        isCanvasShown = false;
        lockCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.EnablePlayerControls();
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

        if (completeAudioSource != null)
        {
            completeAudioSource.Play();
        }

        GridFireplaceManager.Instance.SetInteractionsEnabled(false);

        HideLockCanvas();
        

        if (spawnedChest != null)
        {
            spawnedChest.OpenChest();
        }

        splinde.transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            splinde.transform.DOMoveY(
                splinde.transform.position.y + 0.5f,
                1.5f
            )
        );

        sequence.Append(
            splinde.transform.DOLocalMove(
                Vector3.zero,
                2f
            )
        );

        sequence.Join(
            splinde.transform.DOLocalRotate(
                Vector3.zero,
                3f
            )
        );
    }


}