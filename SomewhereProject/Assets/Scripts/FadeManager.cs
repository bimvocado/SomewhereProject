using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;

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

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public Coroutine FadeOut()
    {
        gameObject.SetActive(true);
        return StartCoroutine(Fade(1f));
    }

    public Coroutine FadeIn()
    {
        gameObject.SetActive(true);
        return StartCoroutine(Fade(0f));
    }

    public void DoFadeOut()
    {
        StartCoroutine(Fade(1f));
    }

    public void DoFadeIn()
    {
        StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        canvasGroup.blocksRaycasts = true;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }
}