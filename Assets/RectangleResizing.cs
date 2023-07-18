using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleResizing : MonoBehaviour, IDragHandler, IEndDragHandler //IScrollHandler
{
    //Drag 
    private RectTransform rectTr;

    //Mouse Scorll Zoom In/Out
    private Vector3 minitialScale;
    private float zoomSpeed = 0.1f;
    private float maxZoom = 10.0f;


    private float initialDistance;

    private void Start()
    {
        rectTr = GetComponent<RectTransform>(); //스크립트 위치의 Rect Transform
        minitialScale = transform.localScale;  //현재 Local Scale을 저장

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTr.anchoredPosition += eventData.delta;  //드래그 이벤트 함수 선언 및 등록
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // rectTr.localPosition = Vector3.zero;  //드래그가 끝났을 때 중심으로 이동
    }

    // OnScroll 나중에 필요하면 구현.
    /*
     public void OnScroll(PointerEventData eventData) //마우스 스크롤 이벤트 등록
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        var desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;
    }
     */


    private Vector3 ClampDesiredScale(Vector3 desiredScale) // 마우스 스크롤의 최대 줌인/아웃
    {
        desiredScale = Vector3.Max(minitialScale, desiredScale);
        desiredScale = Vector3.Min(minitialScale * maxZoom, desiredScale);

        return desiredScale;
    }
}