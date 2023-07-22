using UnityEngine;
using UnityEngine.UI;

public class ObjInstantManager : MonoBehaviour
{
    public GameObject rectanglePrefab;
    public RawImage SpawnPoint;

    public void ObjInstant(Info[] infoArray)
    {
        // using Debug
        for (int i = 0; i < infoArray.Length; i++)
        {
            string logMessage = "";

            logMessage += infoArray[i].id + " Obj, ";
            logMessage += infoArray[i].region_name + ", ";
            foreach (int a in infoArray[i].point)
            {
                logMessage += a.ToString() + ", ";
            }

            Debug.Log(logMessage);

        }

        foreach (Info info in infoArray)
        {
            // Extract the points from the info object
            int x1 = info.point[0];
            int y1 = info.point[1];
            int x2 = info.point[2];
            int y2 = info.point[3];

            // Instantiate a rectangle object from the prefab
            GameObject rectangle = Instantiate(rectanglePrefab, SpawnPoint.transform);

            // Set the parent of the rectangle object
            rectangle.transform.SetParent(SpawnPoint.transform, false);

            // Set the size of the rectangle object with scaling
            RectTransform rectTransform = rectangle.GetComponent<RectTransform>();

            // The width and height of the rectangle
            float rectWidth = x2 - x1;
            float rectHeight = y2 - y1;

            // The position of the center of the rectangle
            Vector2 rectCenter = new Vector2(x1 + rectWidth / 2f, y1 + rectHeight / 2f);

            // Set the size and position of the rectangle
            rectTransform.sizeDelta = new Vector2(rectWidth, rectHeight);
            rectTransform.anchoredPosition = rectCenter + new Vector2(-404, -270); // here is your offset

            // Set the name of the rectangle object
            rectangle.gameObject.name = info.region_name;
            rectangle.layer = LayerMask.NameToLayer("UI");

            Text regionNameText = rectangle.GetComponentInChildren<Text>();
            if (regionNameText != null)
            {
                regionNameText.text = info.region_name;
            }
        }
    }
}

