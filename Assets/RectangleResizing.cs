using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleResizing : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject resizeButtonPrefab;
    public Transform resizeButtonParent;

    private RectTransform rectTransform;
    private Vector2 initialMousePosition;
    private Vector2 initialSize;
    private List<GameObject> resizeButtons = new List<GameObject>(); // list to hold the resize buttons

    public bool resizing;
    private float resizeFactor = 0.3f; // Resizing speed control
    private int cornerIndex;
    private int previousCornerIndex = 1; // New field to keep track of the previously processed corner index

    private const int LEFT_BOTTOM = 0;
    private const int LEFT_TOP = 1;
    private const int RIGHT_TOP = 2;
    private const int RIGHT_BOTTOM = 3;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(1, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        resizing = false;
        DestroyResizeButtons();
        UpdateResizeButtons();
    }

    public void ResizeRectangle(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);
        Vector2 delta = (localPointerPosition - initialMousePosition) * resizeFactor;
        Vector2 newSize;
        Vector3 deltaPosition = Vector3.zero;

        switch (cornerIndex)
        {
            case LEFT_BOTTOM:
                rectTransform.pivot = new Vector2(1, 1);
                newSize = initialSize - delta;
                if (previousCornerIndex == LEFT_TOP) // left_top pivot is 1,0 
                {
                    deltaPosition.y = rectTransform.rect.height;
                }
                else if (previousCornerIndex == RIGHT_TOP) // pivot is 0,0
                {
                    deltaPosition.x = rectTransform.rect.width;
                    deltaPosition.y = rectTransform.rect.height;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM) // pivot is 1,0
                {
                    deltaPosition.x = rectTransform.rect.width;
                }
                break;
            case LEFT_TOP:
                rectTransform.pivot = new Vector2(1, 0);
                delta.y *= -1; // scale
                newSize = initialSize - delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.y = -rectTransform.rect.height;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition.x = rectTransform.rect.width;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition.x = rectTransform.rect.width;
                    deltaPosition.y = -rectTransform.rect.height;
                }
                break;
            case RIGHT_TOP:
                rectTransform.pivot = new Vector2(0, 0);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    deltaPosition.y = -rectTransform.rect.height;
                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition.y = -rectTransform.rect.height;
                }
                break;
            case RIGHT_BOTTOM:
                rectTransform.pivot = new Vector2(0, 1);
                delta.y *= -1; // delta�� y ������ ����
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    deltaPosition.y = rectTransform.rect.height;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition.y = rectTransform.rect.height;
                }
                break;
            default:
                throw new ArgumentException($"Unexpected corner index {cornerIndex}");
        }

        Vector3 oldWorldPosition = rectTransform.localPosition;
        rectTransform.sizeDelta = newSize;
        rectTransform.localPosition = oldWorldPosition + deltaPosition;
        previousCornerIndex = cornerIndex;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        CreateResizeButtons();
    }

    private void CreateResizeButtons()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(resizeButtonPrefab, corners[i], Quaternion.identity, resizeButtonParent);
            button.GetComponent<ResizeButton>().Init(this, i);
            resizeButtons.Add(button);
        }
    }

    public void StartResizing(int cornerIndex)
    {
        StopAllCoroutines();
        this.cornerIndex = cornerIndex;
        this.resizing = true;
        this.initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.initialSize = rectTransform.sizeDelta;
    }

    public void DestroyResizeButtons()
    {
        StopAllCoroutines();
        foreach (GameObject button in resizeButtons)
        {
            Destroy(button);
        }
        resizeButtons.Clear();
    }

    public void UpdateResizeButtons()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            resizeButtons[i].transform.position = corners[i];
        }
    }
}