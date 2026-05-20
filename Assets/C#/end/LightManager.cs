using System.Collections;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] lights;

    [SerializeField] private float targetIntensity = 1f;
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private float countdownDuration = 60f;
    [SerializeField] private float blinkStartTime = 5f;

    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.red;

    [SerializeField] private float blinkSpeed = 0.08f;
    [SerializeField] private float blinkIntensity = 1.5f;

    [SerializeField] private GameObject jumpscareObject;

    public static LightManager instance;

    private Coroutine countdownCoroutine;
    private Coroutine blinkCoroutine;

    private void OnEnable()
    {
        SleepManager.OnSleepStateChanged += StartCountdown;
    }
    private void Awake()
    {
        instance = this;

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }

        foreach (Light lightSource in lights)
        {
            if (lightSource != null)
            {
                lightSource.intensity = 0f;
                lightSource.color = startColor;
            }
        }
    }

    public void TurnOnLights()
    {
        StartCoroutine(FadeLights());
    }

    private IEnumerator FadeLights()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float currentIntensity = Mathf.Lerp(
                0f,
                targetIntensity,
                time / fadeDuration
            );

            foreach (Light lightSource in lights)
            {
                if (lightSource != null)
                {
                    lightSource.intensity = currentIntensity;
                }
            }

            yield return null;
        }

        foreach (Light lightSource in lights)
        {
            if (lightSource != null)
            {
                lightSource.intensity = targetIntensity;
            }
        }
    }

    public void StartCountdown(bool isSleeping)
    {
        if (!isSleeping)
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(CountdownRoutine());
            }
        }
        else
        {
            StopEverything();
        }
    }

    private IEnumerator CountdownRoutine()
    {
        float timer = countdownDuration;
        bool blinkingStarted = false;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            float progress = 1f - (timer / countdownDuration);

            foreach (Light lightSource in lights)
            {
                if (lightSource != null)
                {
                    lightSource.color = Color.Lerp(
                        startColor,
                        endColor,
                        progress
                    );
                }
            }

            if (timer <= blinkStartTime && !blinkingStarted)
            {
                blinkingStarted = true;
                blinkCoroutine = StartCoroutine(BlinkLights());
            }

            yield return null;
        }

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(true);
        }

        countdownCoroutine = null;
    }

    private IEnumerator BlinkLights()
    {
        while (true)
        {
            foreach (Light lightSource in lights)
            {
                if (lightSource != null)
                {
                    lightSource.intensity = 0f;
                }
            }

            yield return new WaitForSeconds(blinkSpeed);

            foreach (Light lightSource in lights)
            {
                if (lightSource != null)
                {
                    lightSource.intensity = blinkIntensity;
                }
            }

            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    private void StopEverything()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (jumpscareObject != null)
        {
            jumpscareObject.SetActive(false);
        }

        ResetLights();
    }

    private void ResetLights()
    {
        foreach (Light lightSource in lights)
        {
            if (lightSource != null)
            {
                lightSource.color = startColor;
                lightSource.intensity = targetIntensity;
            }
        }
    }
}