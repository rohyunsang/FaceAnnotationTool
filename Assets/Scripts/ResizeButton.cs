using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.UI;

public class ResizeButton : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
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
        rectangleResizing.StartResizing(cornerIndex, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectangleResizing.ResizeRectangle(eventData); // 호출 추가
        rectangleResizing.UpdateResizeButtons(); // 버튼 위치 업데이트
        rectangleResizing.resizing = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectangleResizing.resizing = false;
        rectangleResizing.DestroyResizeButtons();
        rectangleResizing.gameObject.GetComponent<Image>().raycastTarget = true;
    }
}