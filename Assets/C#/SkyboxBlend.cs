using UnityEngine;

public class SkyboxSleepBlend : MonoBehaviour
{
    [SerializeField] private Material normalSkybox;
    [SerializeField] private Material sleepSkybox;

    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += HandleSleep;
    }

    private void OnDisable()
    {
        SleepManager.OnSleepStateChanged -= HandleSleep;
    }

    private void HandleSleep(bool isSleeping)
    {
        RenderSettings.skybox = isSleeping ? sleepSkybox : normalSkybox;
    }
}