using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeButton : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectangleResizing rectangleResizing;
    private int cornerIndex;

    public void Init(RectangleResizing rectangleResizing, int cornerIndex)
    {
        this.rectangleResizing = rectangleResizing;
        this.cornerIndex = cornerIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectangleResizing.StartResizing(cornerIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectangleResizing.OnDrag(eventData);
    }
}