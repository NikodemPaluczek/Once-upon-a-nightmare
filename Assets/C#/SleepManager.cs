using System;
using System.Collections;
using UnityEngine;

public class SleepManager : MonoBehaviour
{
    public static SleepManager Instance;

    public static Action<bool> OnSleepStateChanged;
    private bool _isSleeping = true;


    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
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
}