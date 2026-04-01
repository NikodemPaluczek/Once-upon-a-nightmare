using System;
using UnityEngine;

using System;

public static class GameEvents
{
    // bool cuz we checking if we r sleeping
    public static Action<bool> OnSleepStateChanged;

    public static void TriggerSleepState(bool isSleeping)
    {
        OnSleepStateChanged?.Invoke(isSleeping);
    }
}
