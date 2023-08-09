using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RectangleEntry
{
    public string name;
    public List<int> points;
}

[System.Serializable]
public class RectangleData
{
    public List<RectangleEntry> rectangleEntries = new List<RectangleEntry>();
}
[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
    public string userName;
    public string userEmail;
    public string currentTime;
}

public class JsonSerialization : MonoBehaviour
{
    public GameObject jsonParsingObj;
    public GameObject parentPortraits;
    public Text saveText;
    public int saveCount = 0;
    public GameObject UserDataObj;

    public void SaveBtn()
    {
        List<RectangleData> rectangleList = new List<RectangleData>();
        RectangleData rectangleData = new RectangleData();
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];

        foreach (GameObject child in gameObjectList.gameObjects)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            Vector2 pivot = rectTransform.pivot;
            Vector2 adjustedAnchoredPosition = rectTransform.anchoredPosition;

            // 피벗 위치에 따라 anchoredPosition을 조정합니다.
            adjustedAnchoredPosition.x -= pivot.x * rectTransform.rect.width;
            adjustedAnchoredPosition.y -= pivot.y * rectTransform.rect.height;

            RectangleEntry entry = new RectangleEntry();
            entry.name = child.name;
            entry.points = new List<int>
        {
            (int)adjustedAnchoredPosition.x, (int)adjustedAnchoredPosition.y,
            (int)(adjustedAnchoredPosition.x + rectTransform.rect.width), (int)(adjustedAnchoredPosition.y + rectTransform.rect.height)
        };
            rectangleData.rectangleEntries.Add(entry);
        }

        rectangleList.Add(rectangleData);
        
        SerializableList<RectangleData> serializableList = new SerializableList<RectangleData>
        {
            list = rectangleList,
            userName = UserDataObj.GetComponent<SaveUserData>().idCheckText.text,
            userEmail = UserDataObj.GetComponent<SaveUserData>().emailCheckText.text,
            currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string json = JsonUtility.ToJson(serializableList, true);
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
        saveText.text = "완료 : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count.ToString();
    }
}
