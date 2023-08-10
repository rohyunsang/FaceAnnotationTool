using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RectangleEntry
{
    public string name;
    public string points;
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

    private const float PIXEL_WIDTH = 2136f;
    private const float PIXEL_HEIGHT = 3216f;

    private const float PIXEL_FACEIMAGE_WIDTH = 715f;
    private const float PIXEL_FACEIMAGE_HEIGHT = 1080f;
    List<RectangleData> rectangleList = new List<RectangleData>();
    public void SaveBtn()
    {
        
        RectangleData rectangleData = new RectangleData();
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];

        foreach (GameObject child in gameObjectList.gameObjects)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            Vector2 pivot = rectTransform.pivot;

            // Calculate pivot offset
            Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
            Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

            Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
            Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
            Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

            // Convert on-screen position back to original pixel image position
            int originalX1 = (int)(topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY1 = (int)((PIXEL_FACEIMAGE_HEIGHT - topLeft.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
            int originalX2 = (int)(bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY2 = (int)((PIXEL_FACEIMAGE_HEIGHT - bottomRight.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

            RectangleEntry entry = new RectangleEntry();
            entry.name = child.name;
            entry.points =
                originalX1.ToString() +", " + originalY1.ToString() +", " +originalX2.ToString() + ", " + originalY2.ToString();
            rectangleData.rectangleEntries.Add(entry);
        }

        rectangleList.Add(rectangleData);
        saveCount++;
        if (saveCount == jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count)
        {
            SerializableList<RectangleData> serializableList = new SerializableList<RectangleData>
            {
                list = rectangleList,
                userName = UserDataObj.GetComponent<SaveUserData>().idCheckText.text,
                userEmail = UserDataObj.GetComponent<SaveUserData>().emailCheckText.text,
                currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string json = JsonUtility.ToJson(serializableList, true);
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            File.WriteAllText(desktopPath + "/faceField" + ".json", json);
            Debug.Log("Complete");
        }
        
        Transform childTransform = parentPortraits.transform.Find(idx.ToString());

        if (childTransform.gameObject.GetComponent<Portrait>().checkingImage.activeSelf)
        {
            saveCount--;
        }
        childTransform.gameObject.GetComponent<Portrait>().checkingImage.SetActive(true);
        saveText.text = "¿Ï·á : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count.ToString();
    }
}
