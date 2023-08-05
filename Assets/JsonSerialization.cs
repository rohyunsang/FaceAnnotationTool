using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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
    public Text saveText;
    public int saveCount = 0;

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
        saveCount++;
        if (childTransform.gameObject.GetComponent<Portrait>().checkingImage.activeSelf)
        {
            saveCount--;
        }
        childTransform.gameObject.GetComponent<Portrait>().checkingImage.SetActive(true);
        saveText.text = "¿Ï·á : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count.ToString();
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}