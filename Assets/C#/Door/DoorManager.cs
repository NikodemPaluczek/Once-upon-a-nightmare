using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    [SerializeField] private List<GameObject> objects;

    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        ShowCurrentObject();
    }

    public void ShowNextObject()
    {
        if (objects.Count == 0) return;

        currentIndex++;

        if (currentIndex >= objects.Count)
        {
            currentIndex = 0;
        }

        ShowCurrentObject();
    }

    private void ShowCurrentObject()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }
    }
}