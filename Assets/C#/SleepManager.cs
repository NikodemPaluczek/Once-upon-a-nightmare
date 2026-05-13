using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SleepManager : MonoBehaviour
{
    [SerializeField] private GameObject[] sleepObjects;
    [SerializeField] private GameObject[] awakeObjects;

    public static SleepManager Instance;

    public static Action<bool> OnSleepStateChanged;
    private bool _isSleeping = true;


    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        OnSleepStateChanged += SwapObjects;
    }
    private void Start()
    {
        foreach (GameObject awakeObj in awakeObjects)
        {
            awakeObj.SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isSleeping = !_isSleeping;

            StartCoroutine(TriggerSleepState(_isSleeping));
        }
    }

    public static IEnumerator TriggerSleepState(bool isSleeping)
    {
        InputManager.Instance.DisableAllControls();
        ScreenFader.Instance.PlayFade();
        yield return new WaitForSeconds(1);
        OnSleepStateChanged?.Invoke(isSleeping);
        yield return new WaitForSeconds(1); //we wait 1 sec cuz its the length of fade in and fade out
        InputManager.Instance.EnablePlayerControls();
    }

    public void ChangeSleepState()
    {
        _isSleeping = !_isSleeping;
        StartCoroutine(TriggerSleepState(_isSleeping));
    }
    private void SwapObjects(bool isSleeping)
    {
        foreach (GameObject sleepObj  in sleepObjects)
        {
            sleepObj.SetActive(isSleeping);
        }
        foreach (GameObject awakeObj in awakeObjects)
        {
            awakeObj.SetActive(!isSleeping);
        }

    }
}