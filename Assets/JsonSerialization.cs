using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    public GameObject jsonParsingObj;
    public GameObject parentPortraits;

    public void SaveBtn()
    {
        List<RectangleData> rectangleList = new List<RectangleData>();
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];

        foreach (GameObject child in gameObjectList.gameObjects)
        {
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
        File.WriteAllText(desktopPath + "/faceField" + idx.ToString() + ".json", json);
        Debug.Log("Complete");

        Transform childTransform = parentPortraits.transform.Find(idx.ToString());
        childTransform.gameObject.GetComponent<Portrait>().checkingImage.SetActive(true);
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}