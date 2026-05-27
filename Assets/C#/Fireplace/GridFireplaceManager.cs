using System.Collections.Generic;
using UnityEngine;

public class GridFireplaceManager : MonoBehaviour
{
    public GameObject placeholderWin;

    public static GridFireplaceManager Instance;

    [SerializeField] private int rows = 3;
    [SerializeField] private int cols = 3;

    [SerializeField] private FireplaceButton[] allButtons;

    private FireplaceButton[,] grid;
    private int[,] lockCount;

    private List<int> clickOrder = new List<int>();
    private int maxClicks = 3;

    private readonly int[] correctSequence = { 8, 0, 6 };

    public bool interactionsEnabled = false;
    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else Destroy(gameObject);

        grid = new FireplaceButton[rows, cols];
        lockCount = new int[rows, cols];

        BuildGrid();
    }
    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += SetInteractionsEnabled;
    }
    private void Start()
    {
        SetInteractionsEnabled(true);
    }

    private void BuildGrid()
    {
        for (int i = 0; i < allButtons.Length; i++)
        {
            int r = i / cols;
            int c = i % cols;

            grid[r, c] = allButtons[i];
            allButtons[i].SetCoordinates(r, c, i);
        }
    }

    public void ToggleFireplace(int row, int col, int index)
    {
        var tile = grid[row, col];
        if (tile == null || tile.IsBlockedExternally) return;

        bool activating = !tile.IsSelected;

        if (activating)
        {
            if (clickOrder.Count >= maxClicks) return;

            tile.SetSelected(true);
            clickOrder.Add(index);

            AddBlock(row, col);
        }
        else
        {
            tile.SetSelected(false);
            clickOrder.Remove(index);

            RemoveBlock(row, col);
        }

        if (clickOrder.Count == maxClicks)
        {
            ValidateSequence();
        }
    }

    private void AddBlock(int row, int col)
    {
        foreach (var dir in Directions())
        {
            int r = row + dir.x;
            int c = col + dir.y;

            if (InBounds(r, c))
            {
                lockCount[r, c]++;

                grid[r, c].SetBlocked(true);
            }
        }
    }

    private void RemoveBlock(int row, int col)
    {
        foreach (var dir in Directions())
        {
            int r = row + dir.x;
            int c = col + dir.y;

            if (!InBounds(r, c)) continue;

            lockCount[r, c] = Mathf.Max(0, lockCount[r, c] - 1);

            if (lockCount[r, c] == 0)
            {
                grid[r, c].SetBlocked(false);
            }
        }
    }

    private void ValidateSequence()
    {
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (clickOrder[i] != correctSequence[i])
            {
                ResetAll();

                //tutaj wyłączamy grid i włączamy zagadkę na czas

                SetInteractionsEnabled(true);
                TimedLockPuzzleManager.Instance.StartPuzzle();
                return;
            }
        }

        //git wygralismy
        placeholderWin.SetActive(true);
        DoorManager.Instance.ShowNextObject();
    }

    private void ResetAll()
    {
        clickOrder.Clear();
        lockCount = new int[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (grid[r, c] != null)
                    grid[r, c].ForceReset();
            }
        }
    }

    private bool InBounds(int r, int c)
    {
        return r >= 0 && r < rows && c >= 0 && c < cols;
    }

    private Vector2Int[] Directions()
    {
        return new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1)
        };
    }


    public void SetInteractionsEnabled(bool enabled)
    {
        interactionsEnabled = !enabled;

        if (!interactionsEnabled)
        {
            foreach (var btn in allButtons)
            {
                btn.ForceReset(); 
                btn.SetBlocked(true); 
            }
        }
        else
        {
            foreach (var btn in allButtons)
            {
                btn.SetBlocked(false);
            }
        }
    }
}