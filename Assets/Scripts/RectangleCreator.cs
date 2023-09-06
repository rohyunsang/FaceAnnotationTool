using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RectangleCreator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RawImage mainImage; // The image on which rectangles will be created
    public GameObject rectanglePrefab; // The prefab for the rectangle

    private GameObject rectangle;
    private RectTransform currentRectangle;

    private Vector2 originalPosition;
    private Vector2 pivot = new Vector2(0.5f, 0.5f);
    public string curObjName = "";

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        // Create a new rectangle and set its parent to the main image
        rectangle = Instantiate(rectanglePrefab, mainImage.transform);
        currentRectangle = rectangle.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        originalPosition = localPoint;

        // Set the pivot to the bottom left corner
        currentRectangle.pivot = pivot;
        currentRectangle.anchoredPosition = localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the size and position of the rectangle as the user drags the mouse
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        Vector2 direction = localPoint - originalPosition;

        if (direction.x < 0) pivot.x = 1;
        else pivot.x = 0;

        if (direction.y < 0) pivot.y = 1;
        else pivot.y = 0;

        currentRectangle.pivot = pivot;

        // Update the anchoredPosition to the original position because pivot changes can move the rectangle
        currentRectangle.anchoredPosition = originalPosition;

        Vector2 size = localPoint - currentRectangle.anchoredPosition;
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);

        currentRectangle.sizeDelta = size;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectangle.name = curObjName;

        Vector3[] localCorners = new Vector3[4];
        currentRectangle.GetLocalCorners(localCorners);

        for(int i = 0; i < 4; i++)
        {
            Debug.Log(localCorners[i][0] + " " +  localCorners[i][1]);
        }
    }
}