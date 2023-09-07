using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RectangleEntry
{
    public string name;
    public List<int> points = new List<int>();
}

[System.Serializable]
public class ImageData
{
    public string imageName;
    public List<FaceLine> face_line;
    public List<RectangleEntry> rectangleEntries = new List<RectangleEntry>();
}

[System.Serializable]
public class RectangleData
{
    public string imageName;
    public List<RectangleEntry> rectangleEntries = new List<RectangleEntry>();
}

[System.Serializable]
public class SerializableDict
{
    public List<ImageData> imageDataList = new List<ImageData>();
    public string userName;
    public string userEmail;
    public string currentTime;
}

[System.Serializable]
public class CircleData
{
    public string imageName;
    public List<CircleEntry> points = new List<CircleEntry>();

}

[System.Serializable]
public class CircleEntry
{
    public string name;
    public List<int> points = new List<int>();
}

[System.Serializable]
public class SerializableFaceLineDict
{
    public string userName;
    public string userEmail;
    public string currentTime;
    public List<CircleData> circleDataList = new List<CircleData>();
}

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

    private Dictionary<string, List<RectangleEntry>> rectangleDict = new Dictionary<string, List<RectangleEntry>>();
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

    public void InitializeCircleDict()
    {
        PIXEL_FACEIMAGE_WIDTH = PIXEL_WIDTH / PIXEL_HEIGHT * PIXEL_FACEIMAGE_HEIGHT;

        int totalEntries = jsonParsingObj.GetComponent<JsonParsing>().jsonCircles.Count;

        for (int idx = 0; idx < totalEntries; idx++)
        {
            GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonCircles[idx];
            string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

            if (!circleDict.ContainsKey(currentId))
            {
                circleDict[currentId] = new List<CircleEntry>();
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

                int originalX = (originalX1 + originalX2) / 2;
                int originalY = (originalY1 + originalY2) / 2;

                CircleEntry entry = new CircleEntry();
                entry.name = child.name;
                entry.points.Add(originalX);
                entry.points.Add(originalY);

                CircleEntry existingEntry = circleDict[currentId].Find(e => e.name == entry.name);

                if (existingEntry != null)
                {
                    // Overwrite the points for the existing entry
                    existingEntry.points = entry.points;
                }
                else
                {
                    // Add the new entry if it doesn't exist
                    circleDict[currentId].Add(entry);
                }
            }
        }
    }

    public void InitializeRectangleDict()
    {
        PIXEL_FACEIMAGE_WIDTH = PIXEL_WIDTH / PIXEL_HEIGHT * PIXEL_FACEIMAGE_HEIGHT;

        int totalEntries = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count;

        for (int idx = 0; idx < totalEntries; idx++)
        {
            GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares[idx];
            string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;

            if (!rectangleDict.ContainsKey(currentId))
            {
                rectangleDict[currentId] = new List<RectangleEntry>();
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

                RectangleEntry entry = new RectangleEntry();
                entry.name = child.name;

                entry.points.Add(originalX1);
                entry.points.Add(originalY1);
                entry.points.Add(originalX2);
                entry.points.Add(originalY2);

                // Check if an entry with the same name exists
                RectangleEntry existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

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
            rectangleDict[currentId] = new List<RectangleEntry>();
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

            RectangleEntry entry = new RectangleEntry();
            entry.name = child.name;

            entry.points.Add(originalX1);
            entry.points.Add(originalY1);
            entry.points.Add(originalX2);
            entry.points.Add(originalY2);

            // Check if an entry with the same name exists
            RectangleEntry existingEntry = rectangleDict[currentId].Find(e => e.name == entry.name);

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
    public void ExportCircleJson()
    {
        SerializableFaceLineDict serializableDict = new SerializableFaceLineDict()
        {
            userName = UserDataObj.GetComponent<SaveUserData>().idCheckText.text,
            userEmail = UserDataObj.GetComponent<SaveUserData>().emailCheckText.text,
            currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        foreach (var kvp in circleDict)
        {
            CircleData entry = new CircleData
            {
                imageName = kvp.Key,
                points = kvp.Value
            };
            serializableDict.circleDataList.Add(entry);
        }

        string json = JsonUtility.ToJson(serializableDict, true);
        string currentPath = FileBrowserObj.GetComponent<FileBrowserTest>().filePath;

        // Create the 'jsons' directory path.
        string jsonsDirectoryPath = Path.Combine(currentPath, "face_line");
        Directory.CreateDirectory(jsonsDirectoryPath);  // Create the directory if it doesn't exist, otherwise do nothing.

        // Save the .json file inside the 'jsons' directory.
        string jsonFilePath = Path.Combine(jsonsDirectoryPath, "face_line" + ".json");
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("face_line and userName save complete.");

        saveJsonCompleteImage.SetActive(true);
    }
    public void SaveFaceLineAndUserName()
    {
        int totalEntries = jsonParsingObj.GetComponent<JsonParsing>().jsonSquares.Count;
        int idx = jsonParsingObj.GetComponent<JsonParsing>().idx;
        string currentId = jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id;
        if (!circleDict.ContainsKey(currentId))
        {
            circleDict[currentId] = new List<CircleEntry>();
        }
        GameObjectList gameObjectList = jsonParsingObj.GetComponent<JsonParsing>().jsonCircles[idx];

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

            int originalX = (originalX1 + originalX2) / 2;
            int originalY = (originalY1 + originalY2) / 2;

            CircleEntry entry = new CircleEntry();
            entry.name = child.name;
            entry.points.Add(originalX);
            entry.points.Add(originalY);

            CircleEntry existingEntry = circleDict[currentId].Find(e => e.name == entry.name);

            if (existingEntry != null)
            {
                // Overwrite the points for the existing entry
                existingEntry.points = entry.points;
            }
            else
            {
                // Add the new entry if it doesn't exist
                circleDict[currentId].Add(entry);
            }

        }
        saveCount++;
        Transform childTransform = parentPortraits.transform.Find(jsonParsingObj.GetComponent<JsonParsing>().parsedInfo[idx].id);
        if (childTransform.gameObject.GetComponent<Portrait>().checkingImage.activeSelf)
        {
            saveCount--;
        }
        if (saveCount == jsonParsingObj.GetComponent<JsonParsing>().jsonCircles.Count)
        {
            saveCount = 0;
            saveCompleteImage.SetActive(true);
        }

        childTransform.gameObject.GetComponent<Portrait>().checkingImage.SetActive(true);
        saveText.text = "완료 : " + saveCount.ToString() + " / " + jsonParsingObj.GetComponent<JsonParsing>().jsonCircles.Count.ToString();
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