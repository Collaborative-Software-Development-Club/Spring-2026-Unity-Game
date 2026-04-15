using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ScrollContentController : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect;
    public int itemsPerPage = 3;
    public int totalItems = 4;
    public float snapSpeed = 10f;

    private int totalPages;
    private float[] pagePositions;
    private Coroutine snapCoroutine;

    void Start()
    {
        totalPages = Mathf.CeilToInt((float)totalItems / itemsPerPage);

        if (totalPages <= 1)
        {
            pagePositions = new float[] { 0f };
            return;
        }

        pagePositions = new float[totalPages];
        for (int i = 0; i < totalPages; i++)
        {
            pagePositions[i] = (float)i / (totalPages - 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (snapCoroutine != null) StopCoroutine(snapCoroutine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToClosest();
    }

    public void OnScrollbarReleased()
    {
        SnapToClosest();
    }

    public void SnapToClosest()
    {
        if (pagePositions == null || pagePositions.Length == 0) return;

        float current = scrollRect.horizontalNormalizedPosition;
        float closest = pagePositions[0];

        foreach (float pos in pagePositions)
        {
            if (Mathf.Abs(current - pos) < Mathf.Abs(current - closest))
                closest = pos;
        }

        if (snapCoroutine != null) StopCoroutine(snapCoroutine);
        snapCoroutine = StartCoroutine(SnapLerp(closest));
    }

    IEnumerator SnapLerp(float targetPos)
    {
        scrollRect.velocity = Vector2.zero;

        while (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetPos) > 0.001f)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(
                scrollRect.horizontalNormalizedPosition,
                targetPos,
                Time.deltaTime * snapSpeed
            );
            yield return null;
        }
        scrollRect.horizontalNormalizedPosition = targetPos;
    }
}