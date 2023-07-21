using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleResizing : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Vector2 initialMousePosition;
    private Vector2 initialSize;

    private bool resizing;
    private float resizeFactor = 0.1f; // Resizing speed control

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!resizing)
            {
                resizing = true;
                initialMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
                initialSize = rectTransform.sizeDelta;
            }
            ResizeRectangle(eventData);
        }
        else
        {
            resizing = false;
            rectTransform.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        resizing = false;
    }

    private void ResizeRectangle(PointerEventData eventData)
    {
        Vector2 delta = (eventData.position - initialMousePosition) * resizeFactor;
        Vector2 newSize = initialSize + delta;

        rectTransform.sizeDelta = newSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Unused for now but you might want to put something here in the future
    }
}