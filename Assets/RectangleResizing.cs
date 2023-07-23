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

    private bool resizing;
    private float resizeFactor = 0.1f; // Resizing speed control
    private int cornerIndex;
    private int previousCornerIndex = -1; // New field to keep track of the previously processed corner index

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
        if (resizing)
        {
            ResizeRectangle(eventData);
            UpdateResizeButtons();
        }
        else
        {
            rectTransform.anchoredPosition += eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        resizing = false;
        DestroyResizeButtons();
    }

    private void ResizeRectangle(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);
        Vector2 delta = (localPointerPosition - initialMousePosition) * resizeFactor;
        Vector2 newSize;
        Vector3 deltaPosition = Vector3.zero;

        switch (cornerIndex)
        {
            case LEFT_BOTTOM:
                rectTransform.pivot = new Vector2(0, 0);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.y = initialSize.y - newSize.y;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition = initialSize - newSize;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                    deltaPosition.x *= -1;
                }
                break;
            case LEFT_TOP:
                rectTransform.pivot = new Vector2(0, 1);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.y = initialSize.y - newSize.y;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                    deltaPosition.y = 0;
                    deltaPosition *= -1;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition = initialSize - newSize;
                }
                break;
            case RIGHT_TOP:
                rectTransform.pivot = new Vector2(1, 1);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                    deltaPosition.y = 0;
                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                    deltaPosition.y = 0;
                    deltaPosition *= -1;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition = initialSize - newSize;
                    /// this code is wrong
                }
                break;
            case RIGHT_BOTTOM:
                rectTransform.pivot = new Vector2(1, 0);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = initialSize.x - newSize.x;
                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition = initialSize - newSize;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition = initialSize - newSize;
                }
                break;
            default:
                throw new ArgumentException($"Unexpected corner index {cornerIndex}");
        }

        Vector3 oldWorldPosition = rectTransform.position;
        rectTransform.sizeDelta = newSize;
        rectTransform.position = oldWorldPosition - deltaPosition;
        previousCornerIndex = cornerIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DestroyResizeButtons();
        CreateResizeButtons();
    }

    private IEnumerator NoInteractionCoroutine(GameObject button)
    {
        yield return new WaitForSeconds(2f);
        Destroy(button);
        resizeButtons.Remove(button);
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
            StartCoroutine(NoInteractionCoroutine(button));
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

    private void DestroyResizeButtons()
    {
        StopAllCoroutines();
        foreach (GameObject button in resizeButtons)
        {
            Destroy(button);
        }
        resizeButtons.Clear();
    }

    private void UpdateResizeButtons()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        for (int i = 0; i < 4; i++)
        {
            resizeButtons[i].transform.position = corners[i];
        }
    }
}