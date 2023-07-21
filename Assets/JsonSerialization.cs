using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RectangleData
{
    public string name;
    public Vector2 position;
    public float width;
    public float height;
}

public class JsonSerialization : MonoBehaviour
{
    public GameObject faceField;

    public void SaveBtn()
    {
        List<RectangleData> rectangleList = new List<RectangleData>();

        for (int i = 0; i < faceField.transform.childCount; i++)
        {
            GameObject child = faceField.transform.GetChild(i).gameObject;
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            RectangleData rectangleData = new RectangleData();
            rectangleData.name = child.name;
            rectangleData.position = rectTransform.anchoredPosition;
            rectangleData.width = rectTransform.rect.width;
            rectangleData.height = rectTransform.rect.height;
            rectangleList.Add(rectangleData);
        }

        string json = JsonUtility.ToJson(new SerializableList<RectangleData> { list = rectangleList }, true);
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        File.WriteAllText(desktopPath + "/faceField.json", json);

        Debug.Log("Complete");
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}