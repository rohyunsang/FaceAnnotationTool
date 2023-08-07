using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImageRegions
{
    public List<List<int>> forehead;
    public List<List<int>> glabellus;
    public List<List<int>> l_peroucular;
    public List<List<int>> r_peroucular;
    public List<List<int>> l_cheek;
    public List<List<int>> r_cheek;
    public List<List<int>> lip;
    public List<List<int>> chin;
}

public class RootObject : Dictionary<string, ImageRegions> { }

[System.Serializable]
public class Info  //structure
{
    public string id;  // needs int to string 
    public string[] region_name;
    public List<int> point;
}

[System.Serializable]
public class GameObjectList
{
    public List<GameObject> gameObjects = new List<GameObject>();
}

public class JsonParsing : MonoBehaviour
{
    public GameObject PanelManagerObj;
    public GameObject ObjInstantGameObject; // using call ObjInstantManager Class Function
    public RawImage faceImage;

    public GameObject WorkEndImage;

    [SerializeField]
    public List<GameObjectList> jsonSquares = new List<GameObjectList>();
    public List<Texture2D> imageDatas = new List<Texture2D>();
    public List<string> squareCoordinate = new List<string>();
    public List<Info> parsedInfo = new List<Info>();



    public int idx = 0;

    public GameObject portraitPrefab;
    public Transform scrollView;
    public Transform scrollViewInitPanel;

    public GameObject failWindow;
    public Text SquareText;


    public void MakeJsonArray(string jsonData)
    {
        ParseJSONData(jsonData);
    }
    public void MakeImageStringArray(byte[] bytes)
    {
        // Create a Texture2D from the image bytes
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        imageDatas.Add(texture);
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

    public void ClearObjs()
    {
        jsonSquares.Clear();
        imageDatas.Clear();
        parsedInfo.Clear();
        foreach (Transform child in faceImage.transform)
        {
            Destroy(child.gameObject);
        }
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
            portraitInstanceA.name = i.ToString();
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
    public void QueueManager(int idx) // using btn;
    {
        jsonSquares[this.idx].gameObjects.ForEach(square => square.SetActive(false));
        this.idx = idx;
        // 1. 사진을 클릭하면 idx를 기준으로 jsonSquare과 이미지를 뛰운다. 
        jsonSquares[this.idx].gameObjects.ForEach(square => square.SetActive(true));
        faceImage.texture = imageDatas[this.idx];
        SquareText.text = squareCoordinate[idx];
    }


    public void ParseJSONData(string jsonData)
    {
        var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonData);

        // Accessing the data in rootObject
        foreach (var item in rootObject)
        {
            string imageName = item.Key;
            ImageRegions regions = item.Value;

            Info imageInfo = new Info();
            imageInfo.id = imageName;
            imageInfo.region_name = new string[8]; // Assuming there are always 8 regions
            imageInfo.point = new List<int>();

            int i = 0;  // For indexing the region_name array

            // Process each region and add data to the imageInfo object
            imageInfo.region_name[i++] = ProcessRegion("forehead", regions.forehead, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("glabellus", regions.glabellus, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("l_peroucular", regions.l_peroucular, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("r_peroucular", regions.r_peroucular, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("l_cheek", regions.l_cheek, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("r_cheek", regions.r_cheek, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("lip", regions.lip, imageInfo);
            imageInfo.region_name[i++] = ProcessRegion("chin", regions.chin, imageInfo);

            parsedInfo.Add(imageInfo); // Add the created info to the list
        }

        // Optional: Check the data
        foreach (var info in parsedInfo)
        {
            Debug.Log($"ID: {info.id}");
            foreach (var name in info.region_name)
            {
                Debug.Log($"Region Name: {name}");
            }
            foreach (var point in info.point)
            {
                Debug.Log($"Point: {point}");
            }
        }
        

        ObjInstantGameObject.GetComponent<ObjInstantManager>().ObjInstant(parsedInfo);
    }

    private string ProcessRegion(string regionName, List<List<int>> coordinates, Info imageInfo)
    {
        if (coordinates.Count >= 2)
        {
            imageInfo.point.Add(coordinates[0][0]);
            imageInfo.point.Add(coordinates[0][1]);
            imageInfo.point.Add(coordinates[1][0]);
            imageInfo.point.Add(coordinates[1][1]);
        }
        return regionName;
    }
}
