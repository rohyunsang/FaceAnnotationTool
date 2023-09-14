using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class JsonSerialization : MonoBehaviour
{
    public GameObject jsonParsingObj;
    public GameObject parentPortraits;
    public Text saveText;
    public int saveCount = 0;
    public GameObject UserDataObj;

    public float PIXEL_WIDTH = 2136f;
    public float PIXEL_HEIGHT = 3216f;
    public float PIXEL_FACEIMAGE_WIDTH = 715f;
    private const float PIXEL_FACEIMAGE_HEIGHT = 1080f;

    private Dictionary<string, List<RectangleEntryString>> rectangleDict = new Dictionary<string, List<RectangleEntryString>>();
    private Dictionary<string, List<CircleEntry>> circleDict = new Dictionary<string, List<CircleEntry>>();
    public GameObject saveCompleteImage;
    public GameObject saveJsonCompleteImage;

    public GameObject FileBrowserObj;


    public void ClearRectangleDict()
    {
        rectangleDict.Clear();
    }

    public void ClearCircleDict()
    {
        circleDict.Clear();
    }



    public void InitializeRectangleDict() // using button
    {
        PIXEL_FACEIMAGE_WIDTH = PIXEL_WIDTH / PIXEL_HEIGHT * PIXEL_FACEIMAGE_HEIGHT;

        int totalEntries = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count;

        for (int idx = 0; idx < totalEntries; idx++)
        {
            GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];
            string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

            if (!rectangleDict.ContainsKey(currentId))
            {
                rectangleDict[currentId] = new List<RectangleEntryString>();
            }

            foreach (GameObject child in gameObjectList.gameObjects)
            {
                // ... (기존의 사각형 항목을 추가/수정하는 로직)
                RectTransform rectTransform = child.GetComponent<RectTransform>();

                Vector2 pivot = rectTransform.pivot;
                Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
                Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

                Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
                Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
                Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

                int originalX1 = (int)(topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
                int originalY1 = (int)((PIXEL_FACEIMAGE_HEIGHT - topLeft.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
                int originalX2 = (int)(bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
                int originalY2 = (int)((PIXEL_FACEIMAGE_HEIGHT - bottomRight.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

                RectangleEntryString entry = new RectangleEntryString();
                entry.name = child.name;

                entry.points.Add(originalX1.ToString());
                entry.points.Add(originalY1.ToString());
                entry.points.Add(originalX2.ToString());
                entry.points.Add(originalY2.ToString());

                

                // Check if an entry with the same name exists
                RectangleEntryString existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

                if (existingEntry != null)
                {
                    // Overwrite the points for the existing entry
                    existingEntry.points = entry.points;
                }
                else
                {
                    // Add the new entry if it doesn't exist
                    rectangleDict[currentId].Add(entry);
                }
            }
        }
    }

    public void ClearSaveCount()
    {
        saveCount = 0;
        saveText.text = "완료 : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count.ToString();
    }


    public void SaveBtn() // using Rectangle save
    {
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];
        string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

        if (!rectangleDict.ContainsKey(currentId))
        {
            rectangleDict[currentId] = new List<RectangleEntryString>();
        }

        foreach (GameObject child in gameObjectList.gameObjects)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();

            Vector2 pivot = rectTransform.pivot;
            Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
            Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

            Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
            Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
            Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

            int originalX1 = (int)(topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY1 = (int)((PIXEL_FACEIMAGE_HEIGHT - topLeft.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
            int originalX2 = (int)(bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            int originalY2 = (int)((PIXEL_FACEIMAGE_HEIGHT - bottomRight.y) / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

            RectangleEntryString entry = new RectangleEntryString();
            entry.name = child.name;

            if (rectTransform.sizeDelta.x < 0 || rectTransform.sizeDelta.y < 0)
            {
                entry.points.Add("None");
                entry.points.Add("None");
                entry.points.Add("None");
                entry.points.Add("None");
            }
            else
            {
                entry.points.Add(originalX1.ToString());
                entry.points.Add(originalY1.ToString());
                entry.points.Add(originalX2.ToString());
                entry.points.Add(originalY2.ToString());
            }
            // Check if an entry with the same name exists
            RectangleEntryString existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

            if (existingEntry != null)
            {
                // Overwrite the points for the existing entry
                existingEntry.points = entry.points;
            }
            else
            {
                // Add the new entry if it doesn't exist
                rectangleDict[currentId].Add(entry);
            }
        }

        saveCount++;
        Transform childTransform = parentPortraits.transform.Find(jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id);
        if (childTransform.gameObject.GetComponent<Portrait>().checkingImage.activeSelf)
        {
            saveCount--;
        }
        if (saveCount == jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count)
        {
            saveCount = 0;
            saveCompleteImage.SetActive(true);
        }

        childTransform.gameObject.GetComponent<Portrait>().checkingImage.SetActive(true);
        saveText.text = "완료 : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count.ToString();
    }
    public void SaveJson()  // json export
    {

        SerializableDict serializableDict = new SerializableDict
        {
            userName = UserDataObj.GetComponent<SaveUserData>().idCheckText.text,
            userEmail = UserDataObj.GetComponent<SaveUserData>().emailCheckText.text,
            currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        foreach (var kvp in rectangleDict)
        {
            ImageData entry = new ImageData
            {
                imageName = kvp.Key,
                rectangleEntries = kvp.Value
            };
            serializableDict.imageDataList.Add(entry);
        }

        string json = JsonUtility.ToJson(serializableDict, true);
        string currentPath = FileBrowserObj.GetComponent<FileBrowserTest>().filePath;

        // 'jsons' 디렉토리 경로를 생성합니다.
        string jsonsDirectoryPath = Path.Combine(currentPath, "bboxes");
        Directory.CreateDirectory(jsonsDirectoryPath);  // 디렉토리가 없으면 생성하고, 있으면 아무것도 하지 않습니다.

        // 'jsons' 디렉토리 안에 .json 파일을 저장합니다.
        string jsonFilePath = Path.Combine(jsonsDirectoryPath, "bbox" + "_" + System.DateTime.Now.ToString("MM_dd_HH_mm_ss") + ".json");
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("Complete");
    }


    public void OffSaveCompleteImage()
    {
        saveCompleteImage.SetActive(false);
    }
    public void OffSaveJsonCompleteImage()
    {
        saveJsonCompleteImage.SetActive(false);
    }
}