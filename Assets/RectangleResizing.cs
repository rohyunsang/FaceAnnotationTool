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
        rectTr = GetComponent<RectTransform>(); //��ũ��Ʈ ��ġ�� Rect Transform
        minitialScale = transform.localScale;  //���� Local Scale�� ����

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTr.anchoredPosition += eventData.delta;  //�巡�� �̺�Ʈ �Լ� ���� �� ���
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // rectTr.localPosition = Vector3.zero;  //�巡�װ� ������ �� �߽����� �̵�
    }

    // OnScroll ���߿� �ʿ��ϸ� ����.
    /*
     public void OnScroll(PointerEventData eventData) //���콺 ��ũ�� �̺�Ʈ ���
    {
        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        var desiredScale = transform.localScale + delta;

        desiredScale = ClampDesiredScale(desiredScale);

        transform.localScale = desiredScale;
    }
     */


    private Vector3 ClampDesiredScale(Vector3 desiredScale) // ���콺 ��ũ���� �ִ� ����/�ƿ�
    {
        desiredScale = Vector3.Max(minitialScale, desiredScale);
        desiredScale = Vector3.Min(minitialScale * maxZoom, desiredScale);

        return desiredScale;
    }
}