using UnityEngine;

public class DirectionalLightSleepListener : MonoBehaviour
{
    private Light _light;

    private Color _normalColor = Color.white;
    private Color _sleepColor = Color.red;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _normalColor = _light.color;
    }

    private void OnEnable()
    {
        GameEvents.OnSleepStateChanged += HandleSleepState;
    }

    private void OnDisable()
    {
        GameEvents.OnSleepStateChanged -= HandleSleepState;
    }

    private void HandleSleepState(bool isSleeping)
    {
        _light.color = isSleeping ? _sleepColor : _normalColor;
    }
}