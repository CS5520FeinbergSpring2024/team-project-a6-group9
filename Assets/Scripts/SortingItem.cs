using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SortingItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent = null;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SwitchInteractable(bool interactable)
    {
        canvasGroup.blocksRaycasts = interactable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.alpha = .6f;
        SwitchInteractable(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.alpha = 1f;
        SwitchInteractable(true);
        originalParent = null;
    }

    public void SetText(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public Transform GetOriginalParent()
    {
        return originalParent;
    }

    public void SetParent(Transform newParent)
    {
        if (originalParent != null)
        {
            // it's being dragged
            originalParent = newParent;
        }
        else
        {
            transform.SetParent(newParent);
        }
    }
}
