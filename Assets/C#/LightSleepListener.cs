using UnityEngine;

public class LightSleepListener : MonoBehaviour
{
    private Light _light;

    private Color _RealityColor = Color.red;
    private Color _sleepColor = Color.white;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _sleepColor = _light.color;
    }

    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += HandleSleepState;
    }

    private void OnDisable()
    {
        SleepManager.OnSleepStateChanged -= HandleSleepState;
    }

    private void HandleSleepState(bool isSleeping)
    {
        _light.color = isSleeping ? _sleepColor : _RealityColor;
    }
}