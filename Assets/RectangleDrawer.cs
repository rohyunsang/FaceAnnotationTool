using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleDrawer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject rectanglePrefab;
    public Transform parentTransform;

    private Vector2 startPoint;
    private Vector2 endPoint;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        endPoint = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Calculate the size of the rectangle
        Vector2 size = endPoint - startPoint;

        // Instantiate the rectangle prefab as a child of the parent transform
        GameObject rectangle = Instantiate(rectanglePrefab, parentTransform);

        // Set the size and position of the rectangle based on the drag gesture
        RectTransform rectTransform = rectangle.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2f;
    }
}