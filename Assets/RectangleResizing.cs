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
            case 0: // Left bottom
                rectTransform.pivot = new Vector2(0, 0); // Set pivot to the bottom left
                newSize = initialSize - delta;
                if (previousCornerIndex == 1)  //left top
                {
                    deltaPosition.y = initialSize.y - newSize.y;
                }
                else if (previousCornerIndex == 2) // right top
                {
                    deltaPosition = initialSize + newSize;
                }
                else if (previousCornerIndex == 3) //right bottom 
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                }
                break;
            case 1: // Left top
                rectTransform.pivot = new Vector2(0, 1); // Set pivot to the top left
                newSize = initialSize - new Vector2(delta.x, -delta.y);
                if (previousCornerIndex == 0)
                {
                    deltaPosition.y = newSize.y - initialSize.y;
                }
                else if (previousCornerIndex == 2)
                {
                    deltaPosition.x = initialSize.x - newSize.x;
                }
                else if (previousCornerIndex == 3)
                {
                    deltaPosition = newSize - initialSize;
                }
                break;
            case 2: // Right top
                rectTransform.pivot = new Vector2(1, 1); // Set pivot to the top right
                newSize = initialSize + new Vector2(delta.x, -delta.y);
                if (previousCornerIndex == 0)
                {
                    deltaPosition = newSize - initialSize;
                }
                else if (previousCornerIndex == 1)
                {
                    deltaPosition.x = newSize.x - initialSize.x;
                }
                else if (previousCornerIndex == 3)
                {
                    deltaPosition.y = newSize.y - initialSize.y;
                }
                break;
            case 3: // Right bottom
                rectTransform.pivot = new Vector2(1, 0); // Set pivot to the bottom right
                newSize = initialSize + delta;
                if (previousCornerIndex == 0)
                {
                    deltaPosition.x = initialSize.x - newSize.x;
                }
                else if (previousCornerIndex == 1)
                {
                    deltaPosition = initialSize - newSize;
                }
                else if (previousCornerIndex == 2)
                {
                    deltaPosition.y = initialSize.y - newSize.y;
                }
                break;
            default:
                throw new ArgumentException($"Unexpected corner index {cornerIndex}");
        }

        Vector3 oldWorldPosition = rectTransform.position;
        rectTransform.sizeDelta = newSize;

        rectTransform.position = oldWorldPosition - deltaPosition;

        previousCornerIndex = cornerIndex; // Update the previous corner index after processing the current one
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