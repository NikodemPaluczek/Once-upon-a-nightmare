using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playLoop;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private Coroutine fadeCoroutine;
    private float originalVolume;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        originalVolume = audioSource.volume;

        SleepManager.OnSleepStateChanged += SetLoopState;
    }

    private void OnDestroy()
    {
        SleepManager.OnSleepStateChanged -= SetLoopState;
    }

    public void SetLoopState(bool enabled)
    {
        playLoop = enabled;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (playLoop)
        {
            fadeCoroutine = StartCoroutine(FadeIn());
        }
        else
        {
            fadeCoroutine = StartCoroutine(FadeOut());
        }
    }

    public void EnableLoop()
    {
        SetLoopState(true);
    }

    public void DisableLoop()
    {
        SetLoopState(false);
    }

    private IEnumerator FadeIn()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0f;
            audioSource.Play();
        }

        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, originalVolume, elapsed / fadeInDuration);
            yield return null;
        }

        audioSource.volume = originalVolume;
        fadeCoroutine = null;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.volume = originalVolume;
        fadeCoroutine = null;
    }
}