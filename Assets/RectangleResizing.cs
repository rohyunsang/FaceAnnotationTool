using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RectangleResizing : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject resizeButtonPrefab;
    public Transform resizeButtonParent;

    private RectTransform rectTransform;
    private Vector2 initialMousePosition;
    private Vector2 initialSize;
    private List<GameObject> resizeButtons = new List<GameObject>(); // list to hold the resize buttons

    public bool resizing;
    //private float resizeFactor = 0.5f; // Resizing speed control
    private int cornerIndex;
    private int previousCornerIndex = 4; // New field to keep track of the previously processed corner index

    private const int LEFT_BOTTOM = 0;
    private const int LEFT_TOP = 1;
    private const int RIGHT_TOP = 2;
    private const int RIGHT_BOTTOM = 3;
    private const int MIDDLE_CENTOR = 4;

    private const float FACEIMAGE_WIDTH = 715f;
    private const float FACEIMAGE_HEIGHT = 1080f;

    private const float moveSpeed = 0.75f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta * moveSpeed;

        float adjustedPosX = newPos.x - rectTransform.rect.width * rectTransform.pivot.x;
        float adjustedPosY = newPos.y - rectTransform.rect.height * rectTransform.pivot.y;

        // 조정된 경계 체크
        if (adjustedPosX < -FACEIMAGE_WIDTH / 2 || adjustedPosX + rectTransform.rect.width > FACEIMAGE_WIDTH / 2
            || adjustedPosY < -FACEIMAGE_HEIGHT / 2 || adjustedPosY + rectTransform.rect.height > FACEIMAGE_HEIGHT / 2)
        {
            return;
        }

        rectTransform.anchoredPosition = newPos;
        UpdateResizeButtons();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(DelayedDestroyButtons());
    }

    public void ResizeRectangle(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);
        Vector2 delta = (localPointerPosition - initialMousePosition);
        Debug.Log("localPointerPosition: " + localPointerPosition + " inititalMousePosition: " + initialMousePosition);
        Vector2 newSize;
        Vector3 deltaPosition = Vector3.zero;

        switch (cornerIndex)
        {
            case LEFT_BOTTOM:
                rectTransform.pivot = new Vector2(1, 1);
                newSize = initialSize - delta;
                Debug.Log("newSize " + newSize + " initalSize " + initialSize + "delta " + delta);
                if (previousCornerIndex == LEFT_TOP) // left_top pivot is 1,0 
                {
                    deltaPosition.y = rectTransform.rect.height;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == RIGHT_TOP) // pivot is 0,0
                {
                    deltaPosition.x = rectTransform.rect.width;
                    deltaPosition.y = rectTransform.rect.height;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }
                else if (previousCornerIndex == RIGHT_BOTTOM) // pivot is 1,0
                {
                    deltaPosition.x = rectTransform.rect.width;
                    initialMousePosition.x -= deltaPosition.x;
                }
                else if (previousCornerIndex == MIDDLE_CENTOR)  // work correct 
                {
                    deltaPosition.x = rectTransform.rect.width / 2;
                    deltaPosition.y = rectTransform.rect.height / 2;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }

                break;
            case LEFT_TOP:
                rectTransform.pivot = new Vector2(1, 0);
                delta.y *= -1; // scale
                newSize = initialSize - delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.y = -rectTransform.rect.height;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition.x = rectTransform.rect.width;
                    initialMousePosition.x -= deltaPosition.x;

                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition.x = rectTransform.rect.width;
                    deltaPosition.y = -rectTransform.rect.height;
                    initialMousePosition.x -= deltaPosition.x;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == MIDDLE_CENTOR)
                {
                    deltaPosition.x = rectTransform.rect.width / 2;
                    deltaPosition.y = -rectTransform.rect.height / 2;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }

                break;
            case RIGHT_TOP:
                rectTransform.pivot = new Vector2(0, 0);
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    deltaPosition.y = -rectTransform.rect.height;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    initialMousePosition.x -= deltaPosition.x;
                }
                else if (previousCornerIndex == RIGHT_BOTTOM)
                {
                    deltaPosition.y = -rectTransform.rect.height;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == MIDDLE_CENTOR)  // work correct 
                {
                    deltaPosition.x = -rectTransform.rect.width / 2;
                    deltaPosition.y = -rectTransform.rect.height / 2;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }

                break;
            case RIGHT_BOTTOM:
                rectTransform.pivot = new Vector2(0, 1);
                delta.y *= -1; // delta의 y 성분을 반전
                newSize = initialSize + delta;
                if (previousCornerIndex == LEFT_BOTTOM)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    initialMousePosition.x -= deltaPosition.x;

                }
                else if (previousCornerIndex == LEFT_TOP)
                {
                    deltaPosition.x = -rectTransform.rect.width;
                    deltaPosition.y = rectTransform.rect.height;
                    initialMousePosition.x -= deltaPosition.x;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == RIGHT_TOP)
                {
                    deltaPosition.y = rectTransform.rect.height;
                    initialMousePosition.y -= deltaPosition.y;
                }
                else if (previousCornerIndex == MIDDLE_CENTOR)  // work correct 
                {
                    deltaPosition.x = -rectTransform.rect.width / 2;
                    deltaPosition.y = rectTransform.rect.height / 2;
                    initialMousePosition -= new Vector2(deltaPosition.x, deltaPosition.y);
                }

                break;
            default:
                throw new ArgumentException($"Unexpected corner index {cornerIndex}");
        }
        newSize.x = Mathf.Max(newSize.x, 30); // 최소 너비
        newSize.y = Mathf.Max(newSize.y, 30); // 최소 높이

        Vector3 oldWorldPosition = rectTransform.localPosition;
        Vector3 newPosition = oldWorldPosition + deltaPosition;


        float adjustedPosX = newPosition.x - newSize.x * rectTransform.pivot.x;
        float adjustedPosY = newPosition.y - newSize.y * rectTransform.pivot.y;

        // 조정된 경계 체크
        if (adjustedPosX < -600 || adjustedPosX + newSize.x > 600
            || adjustedPosY < -480 || adjustedPosY + newSize.y > 480)
        {
            return;
        }

        rectTransform.sizeDelta = newSize;
        rectTransform.localPosition = newPosition;
        previousCornerIndex = cornerIndex;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (resizeButtons.Count == 0) // If i double click fastly then make two 4 boxes Prevent
        {
            CreateResizeButtons();
        }
        StartCoroutine(DelayedDestroyButtons());
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        DestroyResizeButtons();
    }

    private IEnumerator DelayedDestroyButtons()
    {
        yield return new WaitForSeconds(1f);
        if (!resizing)
        {
            DestroyResizeButtons();
        }
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

    public void StartResizing(int cornerIndex, PointerEventData eventData)
    {
        StopAllCoroutines();
        this.cornerIndex = cornerIndex;
        this.resizing = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out this.initialMousePosition);
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