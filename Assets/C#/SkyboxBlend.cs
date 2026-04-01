using UnityEngine;

public class SkyboxSleepBlend : MonoBehaviour
{
    [SerializeField] private Material normalSkybox;
    [SerializeField] private Material sleepSkybox;

    private void OnEnable()
    {
        GameEvents.OnSleepStateChanged += HandleSleep;
    }

    private void OnDisable()
    {
        GameEvents.OnSleepStateChanged -= HandleSleep;
    }

    private void HandleSleep(bool isSleeping)
    {
        RenderSettings.skybox = isSleeping ? sleepSkybox : normalSkybox;
    }
}