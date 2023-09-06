using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleMoving : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta;
        rectTransform.anchoredPosition = newPos;
    }
}