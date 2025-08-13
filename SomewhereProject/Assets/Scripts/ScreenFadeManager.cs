using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public class ScreenFadeManager : MonoBehaviour
{
    public static ScreenFadeManager Instance { get; private set; }

    [SerializeField]
    private Image fadeImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeOut(float duration = 1.0f, Action onStart = null, Action onComplete = null)
    {
        if (fadeImage == null) return;
        StartCoroutine(Fade(0f, 1f, duration, onStart, onComplete));
    }

    public void FadeIn(float duration = 1.0f, Action onStart = null, Action onComplete = null)
    {
        if (fadeImage == null) return;
        StartCoroutine(Fade(1f, 0f, duration, onStart, onComplete));
    }

    public void FadeInOut(float outDuration = 0.7f, float inDuration = 0.7f, float delay = 0.2f, Action onStart = null, Action onComplete = null)
    {
        if (fadeImage == null) return;
        StartCoroutine(PerformFadeInOut(outDuration, inDuration, delay, onStart, onComplete));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration, Action onStart, Action onComplete)
    {
        onStart?.Invoke();

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        float time = 0f;
        Color color = fadeImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        if (endAlpha == 0f)
        {
            fadeImage.raycastTarget = false;
            fadeImage.gameObject.SetActive(false);
        }

        onComplete?.Invoke();
    }

    private IEnumerator PerformFadeInOut(float outDuration, float inDuration, float delay, Action onStart, Action onComplete)
    {
        yield return StartCoroutine(Fade(0f, 1f, outDuration, onStart, null));
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Fade(1f, 0f, inDuration, null, onComplete));
    }
}