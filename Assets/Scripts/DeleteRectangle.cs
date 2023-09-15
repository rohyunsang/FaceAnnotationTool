using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteRectangle : MonoBehaviour, IPointerDownHandler
{
    public GameObject deleteButtonPrefab; // Reference to the deleteButton prefab
    public GraphicRaycaster graphicRaycaster;
    public GameObject rectUndoObj;
    public GameObject faceImage;

    private void Start()
    {
        rectUndoObj = GameObject.Find("UndoManager");
        faceImage = GameObject.Find("faceImage");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Check for overlapping UI elements using raycasting
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            // Check if the Rectangle was hit
            if (results.Any(result => result.gameObject.CompareTag("Rectangle")))
            {
                GameObject rectangle = results.First(result => result.gameObject.CompareTag("Rectangle")).gameObject;

                // Instantiate the deleteButton prefab and position it next to the rectangle
                GameObject deleteButtonInstance = Instantiate(deleteButtonPrefab, rectangle.transform.parent);
                deleteButtonInstance.GetComponent<RectTransform>().SetAsLastSibling();

                // Set the position of the deleteButton relative to where the rectangle was clicked
                Vector2 localPointerPosition = ConvertToBackgroundLocalCoordinates(eventData.position, faceImage.transform, deleteButtonInstance.transform);
                deleteButtonInstance.GetComponent<RectTransform>().anchoredPosition = localPointerPosition + new Vector2(-250f, -440f); // Adjust as needed

                // Add a listener to the Delete button to destroy the circle when clicked
                Button deleteButton = deleteButtonInstance.transform.Find("DeleteButton").GetComponent<Button>();
                if (deleteButton != null)
                {
                    deleteButton.onClick.AddListener(() => {
                        RectTransform rectTransform = rectangle.GetComponent<RectTransform>();
                        if (rectTransform != null)
                        {
                            rectUndoObj.GetComponent<RectUndo>().originalSize.Push(rectTransform.sizeDelta);
                            rectUndoObj.GetComponent<RectUndo>().undoRectangle.Push(rectangle);
                            rectTransform.sizeDelta = new Vector2(-1, -1);
                        }

                        // rectangle의 자식 오브젝트에서 Text 컴포넌트를 찾아 값을 ""로 설정한다.
                        Text childText = rectangle.GetComponentInChildren<Text>();
                        if (childText != null)
                        {
                            childText.text = "";
                        }
                        Destroy(deleteButtonInstance);
                    });
                }

                Button cancelButton = deleteButtonInstance.transform.Find("CancelButton").GetComponent<Button>();
                if (cancelButton != null)
                {
                    cancelButton.onClick.AddListener(() => Destroy(deleteButtonInstance));
                }
            }
        }
    }

    private Vector2 ConvertToBackgroundLocalCoordinates(Vector2 originalLocalPos, Transform originalParent, Transform targetParent)
    {
        Vector3 worldPos = originalParent.TransformPoint(originalLocalPos); // Convert to world coordinates
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, worldPos); // Convert world to screen

        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetParent as RectTransform, screenPos, null, out Vector2 localPos); // Convert screen to local of target

        return localPos;
    }
}