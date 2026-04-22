using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    static ScreenFadeController instance;

    Canvas overlayCanvas;
    CanvasGroup overlayCanvasGroup;
    Image overlayImage;
    Coroutine fadeCoroutine;

    public static ScreenFadeController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject controllerObject = new GameObject("ScreenFadeController");
                instance = controllerObject.AddComponent<ScreenFadeController>();
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        EnsureOverlay();
    }

    public void FadeInFromBlack(float durationSeconds)
    {
        EnsureOverlay();
        SetOverlayAlpha(1f);
        StartFade(0f, durationSeconds);
    }

    public void FadeOutToBlack(float durationSeconds)
    {
        EnsureOverlay();
        StartFade(1f, durationSeconds);
    }

    void EnsureOverlay()
    {
        if (overlayCanvas != null && overlayCanvasGroup != null && overlayImage != null)
        {
            return;
        }

        overlayCanvas = gameObject.GetComponent<Canvas>();
        if (overlayCanvas == null)
        {
            overlayCanvas = gameObject.AddComponent<Canvas>();
        }

        overlayCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        overlayCanvas.sortingOrder = 5000;

        if (gameObject.GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }

        overlayCanvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (overlayCanvasGroup == null)
        {
            overlayCanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        overlayCanvasGroup.interactable = false;
        overlayCanvasGroup.blocksRaycasts = false;

        Transform existingChild = transform.Find("FadeImage");
        if (existingChild == null)
        {
            GameObject imageObject = new GameObject("FadeImage");
            imageObject.transform.SetParent(transform, false);
            overlayImage = imageObject.AddComponent<Image>();
        }
        else
        {
            overlayImage = existingChild.GetComponent<Image>();
            if (overlayImage == null)
            {
                overlayImage = existingChild.gameObject.AddComponent<Image>();
            }
        }

        overlayImage.color = Color.black;

        RectTransform imageRect = overlayImage.rectTransform;
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        if (overlayCanvasGroup.alpha <= 0f)
        {
            overlayCanvasGroup.alpha = 0f;
        }
    }

    void SetOverlayAlpha(float alpha)
    {
        EnsureOverlay();
        overlayCanvasGroup.alpha = Mathf.Clamp01(alpha);
    }

    void StartFade(float targetAlpha, float durationSeconds)
    {
        EnsureOverlay();

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, Mathf.Max(0f, durationSeconds)));
    }

    System.Collections.IEnumerator FadeRoutine(float targetAlpha, float durationSeconds)
    {
        float startAlpha = overlayCanvasGroup.alpha;

        if (durationSeconds <= 0f)
        {
            overlayCanvasGroup.alpha = targetAlpha;
            fadeCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < durationSeconds)
        {
            elapsed += Time.deltaTime;
            overlayCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / durationSeconds);
            yield return null;
        }

        overlayCanvasGroup.alpha = targetAlpha;
        fadeCoroutine = null;
    }
}