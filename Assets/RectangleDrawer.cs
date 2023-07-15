using UnityEngine;
using UnityEngine.UI;

public class RectangleDrawer : MonoBehaviour
{
    public RawImage drawingArea;
    private RectTransform rectTransform;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private GameObject rectangleObject;

    private Vector2 mPosCur;   // �ǽð�(���� ������) ���콺 ��ǥ
    private Vector2 mPosBegin; // �巡�� ���� ���� ���콺 ��ǥ
    private Vector2 mPosMin;   // Rect�� �ּ� ���� ��ǥ
    private Vector2 mPosMax;   // Rect�� �ִ� ���� ��ǥ
    private bool showSelection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }

        if (rectTransform != null)
        {
            UpdateRectangle();
        }

        ////////////////////////////////////////////////////////////////

        showSelection = Input.GetMouseButton(0);
        if (!showSelection) return;

        mPosCur = Input.mousePosition;
        mPosCur.y = Screen.height - mPosCur.y; // Y ��ǥ(����) ����

        if (Input.GetMouseButtonDown(0))
        {
            mPosBegin = mPosCur;
        }

        mPosMin = Vector2.Min(mPosCur, mPosBegin);
        mPosMax = Vector2.Max(mPosCur, mPosBegin);
    }

    private void OnGUI()
    {
        if (!showSelection) return;
        Rect rect = new Rect();
        rect.min = mPosMin;
        rect.max = mPosMax;

        GUI.Box(rect, "");
    }

    private void StartDrawing()
    {
        startPoint = Input.mousePosition;
        Debug.Log(startPoint);
        rectangleObject = new GameObject("Rectangle");
        rectTransform = rectangleObject.AddComponent<RectTransform>();
        rectTransform.SetParent(drawingArea.transform, false);

        // Add Image component to the rectangleObject
        Image rectangleImage = rectangleObject.AddComponent<Image>();

        // Set the color of the rectangle
        rectangleImage.color = Color.red; // Change the color to your desired color
    }

    private void StopDrawing()
    {
        endPoint = Input.mousePosition;
        Vector2 size = endPoint - startPoint;
        rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2f;
        rectTransform = null;
    }

    private void UpdateRectangle()
    {
        endPoint = Input.mousePosition;
        Vector2 size = endPoint - startPoint;
        rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        rectTransform.anchoredPosition = (startPoint + endPoint) / 2f;
    }
}