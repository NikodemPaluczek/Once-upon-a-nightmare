using System;
using UnityEngine;
using System.Collections;

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance;
    
    [SerializeField] private JournalPage[] pages;
    [SerializeField] private JournalEntryBank entryBank;

    [Header("UI Spreads")]
    [SerializeField] private RectTransform[] spreads;

    [Header("Notification UI")]
    [SerializeField] private GameObject notification;

    private int currentSpreadIndex = 0;
    private int entryCount = 0;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateSpreads();

        if (notification != null)
            notification.SetActive(false);
    }

    public void NextPage()
    {
        if (currentSpreadIndex + 1 >= spreads.Length)
            return;

        currentSpreadIndex++;
        UpdateSpreads();
    }

    public void PreviousPage()
    {
        if (currentSpreadIndex - 1 < 0)
            return;

        currentSpreadIndex--;
        UpdateSpreads();
    }

    private void UpdateSpreads()
    {
        for (int i = 0; i < spreads.Length; i++)
        {
            spreads[i].gameObject.SetActive(i == currentSpreadIndex);
        }
    }

    public void AddEntry(string entryId)
    {
        string content = entryBank.GetEntry(entryId);

        if (string.IsNullOrEmpty(content))
        {
            Debug.LogWarning("Entry content is empty or not found.");
            return;
        }

        foreach (var page in pages)
        {
            if (!page.IsOccupied)
            {
                page.SetEntry(content);
                entryCount++;

                if (entryCount % 3 == 0)
                {
                    NextPage();
                }

                ShowNotification();

                return;
            }
        }

        Debug.LogWarning("No free journal pages available.");
    }

    private void ShowNotification()
    {
        if (notification == null)
            return;

        StopAllCoroutines();
        StartCoroutine(NotificationRoutine());
    }

    private IEnumerator NotificationRoutine()
    {
        notification.SetActive(true);

        yield return new WaitForSeconds(5f);

        notification.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddEntry("Cage");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddEntry("Candle");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPage();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPage();
        }
    }
}