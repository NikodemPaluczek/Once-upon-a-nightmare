using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    [SerializeField] private Image fadeImage;
    [SerializeField] float duration = 1.0f;

    [SerializeField] private GameObject FadeCanvasObj;

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
    public void PlayFade()
    {
        FadeCanvasObj.SetActive(true);
        StartCoroutine(FadeSequence());
    }
    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = t/duration;
            fadeImage.color = c;
            yield return null;
        }
        c.a = 1f;
        fadeImage.color = c;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = 1f - (t/duration);
            fadeImage.color = c;
            yield return null;
        }
        c.a = 0f;
        fadeImage.color = c;
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(FadeSequence());
        }
    }
}
