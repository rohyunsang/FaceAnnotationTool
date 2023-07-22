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

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
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
        Vector2 delta = (eventData.position - initialMousePosition) * resizeFactor;

        if (cornerIndex == 0 || cornerIndex == 1) // Left vertices
        {
            if(rectTransform.pivot.x == 0)
            {

            }
            Vector2 pivotPoint = rectTransform.pivot;
            pivotPoint.x = 1; // Set pivot to the right side
            rectTransform.pivot = pivotPoint;

            Vector2 newSize = initialSize - delta;
            rectTransform.sizeDelta = newSize;
        }
        else if (cornerIndex == 2 || cornerIndex == 3) // Right vertices
        {
            Vector2 pivotPoint = rectTransform.pivot;
            pivotPoint.x = 0; // Set pivot to the left side
            rectTransform.pivot = pivotPoint;

            Vector2 newSize = initialSize + delta;
            rectTransform.sizeDelta = newSize;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DestroyResizeButtons();
        CreateResizeButtons();
    }

    private void CreateResizeButtons()
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        //Debug.Log(corners);

        for (int i = 0; i < 4; i++)
        {
            GameObject button = Instantiate(resizeButtonPrefab, corners[i], Quaternion.identity, resizeButtonParent);
            button.GetComponent<ResizeButton>().Init(this, i);
            resizeButtons.Add(button);
        }
    }

    public void StartResizing(int cornerIndex)
    {
        this.cornerIndex = cornerIndex;
        this.resizing = true;
        this.initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.initialSize = rectTransform.sizeDelta;
    }

    private void DestroyResizeButtons()
    {
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