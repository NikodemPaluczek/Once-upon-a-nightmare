using UnityEngine;

public class SleepManager : MonoBehaviour
{
    private bool _isSleeping = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isSleeping = !_isSleeping;

            GameEvents.TriggerSleepState(_isSleeping);
        }
    }
}