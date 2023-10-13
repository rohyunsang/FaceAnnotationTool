using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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


[System.Serializable]
public class RectangleEntryString
{
    public string name;
    public List<string> points = new List<string>();
}


[System.Serializable]
public class ImageData
{
    public string imageName;
    public List<RectangleEntryString> rectangleEntries = new List<RectangleEntryString>();
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
public class RectangleEntry
{
    public string name;
    public List<int> points;
}

[System.Serializable]
public class FaceLine
{
    public List<int> points;
}

[System.Serializable]
public class ImageDataFile
{
    public string imageName;
    public List<FaceLine> face_line;
    public List<RectangleEntry> rectangleEntries;
}

[System.Serializable]
public class ImageDataFileString
{
    public string imageName;
    public List<FaceLine> face_line;
    public List<RectangleEntryString> rectangleEntries;
}



[System.Serializable]
public class RootObject // integer
{
    public List<ImageDataFile> imageDataList;
}
public class RootStringObject // string
{
    public List<ImageDataFileString> imageDataList;
}


[System.Serializable]
public class Info  //structure
{
    public string id;  // needs int to string
    public string[] region_name;
    public List<int> point;
    public List<int> faceLinePoints;
}

[System.Serializable]
public class GameObjectList  // using jsonCircle and jsonRectangle
{
    public List<GameObject> gameObjects = new List<GameObject>();
}

public class JsonParsing : MonoBehaviour
{
    public GameObject PanelManagerObj;
    public GameObject ObjInstantGameObject; // using call ObjInstantManager Class Function
    public RawImage faceImage;
    public GameObject serialJsonObj;

    [SerializeField]
    public List<GameObjectList> jsonSquares = new List<GameObjectList>();
    [SerializeField]
    public List<GameObjectList> jsonCircles = new List<GameObjectList>();
    public List<Texture2D> imageDatas = new List<Texture2D>();
    public List<Info> parsedInfo = new List<Info>();

    public int idx = 0;

    public GameObject portraitPrefab;
    public Transform scrollView;
    public Transform scrollViewInitPanel;

    public GameObject failWindow;

    private bool isCoroutineRunning = false;

    // using UI Renderer
    public GameObject rectUndoObj;

    public List<int> imageWidths = new List<int>();
    public List<int> imageHeights = new List<int>();

    public void MakeJsonArray(string jsonData)
    {
        ParsingStringJSONDATA(jsonData);
    }
    public void MakeImageStringArray(byte[] bytes)
    {
        // Create a Texture2D from the image bytes
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        imageDatas.Add(texture);
        
        imageWidths.Add(texture.width);
        imageHeights.Add(texture.height);

    }

    public void CheckingFileCount()
    {
        if (jsonSquares.Count == imageDatas.Count && jsonSquares.Count != 0)
        {
            InitPortrait();
        }
        else
        {
            failWindow.SetActive(true);
            Invoke("FailWindowSetActiveFalse", 3f);
            ClearObjs();
        }
    }

    public void ClearFaceLine()
    {
        jsonCircles.Clear();
    }
    public void ClearBBOX()
    {
        jsonSquares.Clear();
    }


    public void ClearObjs()
    {
        this.idx = 0; // prevent queueManager out of range
        jsonSquares.Clear();
        jsonCircles.Clear();
        imageDatas.Clear();
        parsedInfo.Clear();
        foreach (Transform child in faceImage.transform)
        {
            if (child.name.Contains("UI"))
                continue;
            Destroy(child.gameObject);
        }
        isCoroutineRunning = false;
        imageHeights.Clear();
        imageWidths.Clear();
    }

    private void FailWindowSetActiveFalse()
    {
        failWindow.SetActive(false);
    }
    public void Portrait()
    {
        for (int i = 0; i < imageDatas.Count; i++)
        {
            GameObject portraitInstanceA = Instantiate(portraitPrefab, scrollView.transform);
            portraitInstanceA.name = parsedInfo[i].id;
            portraitInstanceA.GetComponent<Image>().sprite = Sprite.Create(imageDatas[i], new Rect(0, 0, imageDatas[i].width, imageDatas[i].height), Vector2.one * 0.5f);
        }
    }
    public void InitPortrait()
    {
        Debug.Log("InitPortrait");
        for (int i = 0; i < imageDatas.Count; i++)
        {
            GameObject portraitInstanceB = Instantiate(portraitPrefab, scrollViewInitPanel.transform);
            portraitInstanceB.name = i.ToString();
            portraitInstanceB.GetComponent<Image>().sprite = Sprite.Create(imageDatas[i], new Rect(0, 0, imageDatas[i].width, imageDatas[i].height), Vector2.one * 0.5f);
            Destroy(portraitInstanceB.GetComponent<Button>());
        }
    }


    public void RectanglesSetActiveFalse()
    {
        foreach (GameObjectList gameObjectList in jsonSquares)
        {
            for (int i = 0; i < gameObjectList.gameObjects.Count; i++)
            {
                gameObjectList.gameObjects.ForEach(square => square.SetActive(false));
            }
        }
    }
    public void CirclesSetActiveFalse()  // reference button
    {
        foreach (GameObjectList gameObjectList in jsonCircles)
        {
            for (int i = 0; i < gameObjectList.gameObjects.Count; i++)
            {
                gameObjectList.gameObjects.ForEach(circle => circle.SetActive(false));
            }
        }
    }
    public void QueueManager(int idx) // using btn;
    {
        rectUndoObj.GetComponent<RectUndo>().ClearStack();
        
        if (jsonSquares.Count != 0)
            jsonSquares[this.idx].gameObjects.ForEach(square => square.SetActive(false));
        
        this.idx = idx;
        if (jsonSquares.Count != 0)
            jsonSquares[this.idx].gameObjects.ForEach(square => square.SetActive(true));
        faceImage.texture = imageDatas[this.idx];
        if (!isCoroutineRunning && jsonSquares.Count != 0)
        {
            StartCoroutine(SetRaycastTargetTrueEveryTwoSecond());
        }

        //change faceImage ratio
        faceImage.GetComponent<RectTransform>().sizeDelta = new Vector2((float)imageWidths[idx] / imageHeights[idx] * 1080f, 
                                                            faceImage.GetComponent<RectTransform>().sizeDelta.y);
    }

    private IEnumerator SetRaycastTargetTrueEveryTwoSecond()
    {
        isCoroutineRunning = true;
        while (true)
        {
            foreach (GameObjectList list in jsonSquares)
            {
                foreach (GameObject obj in list.gameObjects)
                {
                    Image img = obj.GetComponent<Image>();
                    if (img != null)
                    {
                        img.raycastTarget = true;
                    }

                    RawImage rawImg = obj.GetComponent<RawImage>();
                    if (rawImg != null)
                    {
                        rawImg.raycastTarget = true;
                    }
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }

    public void ParsingStringJSONDATA(string jsonData)
    {
        var rootObject = JsonConvert.DeserializeObject<RootStringObject>(jsonData);

        foreach (var imageData in rootObject.imageDataList)
        {
            Info imageInfo = new Info();
            imageInfo.id = imageData.imageName;
            imageInfo.region_name = new string[imageData.rectangleEntries.Count];
            imageInfo.point = new List<int>();
            imageInfo.faceLinePoints = new List<int>();

            if (imageData.face_line != null)
            {
                // Process face_line data
                foreach (var faceLine in imageData.face_line)
                {
                    imageInfo.faceLinePoints.AddRange(faceLine.points);
                }
            }

            int i = 0;  
            foreach (var rectangleEntry in imageData.rectangleEntries)
            {
                imageInfo.region_name[i] = rectangleEntry.name;
                List<int> list = new List<int>();
                foreach (string s in rectangleEntry.points)
                {
                    if (s.Contains("None"))
                        list.Add(0);
                    else
                        list.Add(int.Parse(s));
                }
                imageInfo.point.AddRange(list);
                i++;
            }
            parsedInfo.Add(imageInfo); // ������ ������ ��Ͽ� �߰�
        }
        parsedInfo.Sort((info1, info2) => string.Compare(info1.id, info2.id));
        ObjInstantGameObject.GetComponent<ObjInstantManager>().ObjRectangleInstant(parsedInfo);
    }

    public void ParseJSONData(string jsonData)  //using integer
    {
        var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);

        // rootObject���� �����Ϳ� �׼���
        foreach (var imageData in rootObject.imageDataList)
        {
            Info imageInfo = new Info();
            imageInfo.id = imageData.imageName;
            imageInfo.region_name = new string[imageData.rectangleEntries.Count];
            imageInfo.point = new List<int>();
            imageInfo.faceLinePoints = new List<int>();

            if (imageData.face_line != null)
            {
                // Process face_line data
                foreach (var faceLine in imageData.face_line)
                {
                    imageInfo.faceLinePoints.AddRange(faceLine.points);
                }
            }

            int i = 0;  // region_name �迭 �ε����� ���� ����

            // �� rectangleEntry�� ó���ϰ� imageInfo ��ü�� ������ �߰�
            foreach (var rectangleEntry in imageData.rectangleEntries)
            {
                imageInfo.region_name[i] = rectangleEntry.name;
                imageInfo.point.AddRange(rectangleEntry.points);
                i++;
            }
            parsedInfo.Add(imageInfo); // ������ ������ ��Ͽ� �߰�
        }
        parsedInfo.Sort((info1, info2) => string.Compare(info1.id, info2.id));
        ObjInstantGameObject.GetComponent<ObjInstantManager>().ObjRectangleInstant(parsedInfo);
    }

    public void OnOffFaceLine()
    {
        foreach (GameObjectList gameObjectList in jsonCircles)
        {
            for (int i = 0; i < gameObjectList.gameObjects.Count; i++)
            {
                GameObject circle = gameObjectList.gameObjects[i];

                // GameObject�� ���� Ȱ��ȭ ���¸� Ȯ���ϰ� �� �ݴ� ���·� �����մϴ�.
                circle.SetActive(!circle.activeSelf);
            }
        }
    }
}