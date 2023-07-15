using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RectangleCreator : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RawImage mainImage; // The image on which rectangles will be created
    public GameObject rectanglePrefab; // The prefab for the rectangle

    private RectTransform currentRectangle;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create a new rectangle and set its parent to the main image
        GameObject rectangle = Instantiate(rectanglePrefab, mainImage.transform);
        currentRectangle = rectangle.GetComponent<RectTransform>();

        // Set the start position of the rectangle
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        currentRectangle.anchoredPosition = localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the size and position of the rectangle as the user drags the mouse
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        currentRectangle.sizeDelta = localPoint - currentRectangle.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Finish the drag operation, we can add additional code here if necessary
    }
}