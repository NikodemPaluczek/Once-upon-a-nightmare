using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [Header("Fade Images")]
    [SerializeField] private Image fadeImage1;
    [SerializeField] private Image fadeImage2;

    [SerializeField] private float duration = 1.0f;
    [SerializeField] private GameObject FadeCanvasObj;

    private Image currentFadeImage;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Awake()
    {
        FadeCanvasObj.SetActive(false);
    }

    // fadeType = 1 albo 2
    public void PlayFade(int fadeType)
    {
        FadeCanvasObj.SetActive(true);

        switch (fadeType)
        {
            case 1:
                currentFadeImage = fadeImage1;
                break;

            case 2:
                currentFadeImage = fadeImage2;
                break;

            default:
                Debug.LogWarning("Niepoprawny fadeType!");
                return;
        }

        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = currentFadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t / duration;
            currentFadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        currentFadeImage.color = c;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = currentFadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t / duration);
            currentFadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        currentFadeImage.color = c;
    }

    public IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(3f);

        FadeCanvasObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayFade(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayFade(2);
        }
    }
}