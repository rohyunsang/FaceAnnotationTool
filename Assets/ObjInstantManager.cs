using System.Collections;
using System.Collections.Generic;
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
            
            Debug.Log(infoArray[i].id + " Obj");
            Debug.Log(infoArray[i].region_name);
            foreach (int a in infoArray[i].point)
            {
                Debug.Log(a);
            }
            Debug.Log(""); // Add an empty line between each Info object
        }

        // Get the dimensions of the SpawnPoint
        RectTransform spawnPointRect = SpawnPoint.GetComponent<RectTransform>();
        float spawnPointWidth = spawnPointRect.rect.width;
        float spawnPointHeight = spawnPointRect.rect.height;
        Vector2 spawnPointCenter = new Vector2(spawnPointRect.position.x, spawnPointRect.position.y - spawnPointHeight / 2f);

        foreach (Info info in infoArray)
        {
            // Extract the points from the info object
            int x1 = info.point[0];
            int y1 = info.point[1];
            int x2 = info.point[2];
            int y2 = info.point[3];

            
            // Instantiate a rectangle object from the prefab
            GameObject rectangle = Instantiate(rectanglePrefab, SpawnPoint.transform);

            rectangle.transform.SetParent(SpawnPoint.transform, false);

            // Set the size of the rectangle object with scaling
            RectTransform rectTransform = rectangle.GetComponent<RectTransform>();

            // Set other properties of the rectangle object as needed
            // For example, you can set the ID and region name as text on the UI element

            rectangle.gameObject.name = info.region_name;
        }
    }
}
