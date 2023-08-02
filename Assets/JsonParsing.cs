using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class JSONData  // use json Parsing point is error so just split ','
{
    public int[] id;
    public string[] region_name;
}

[System.Serializable]
public class Info  //structure
{
    public string id;  // needs int to string 
    public string region_name;
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

    [SerializeField]
    private Info[] infoArray = new Info[8];

    public int idx = 0;

    public GameObject portraitPrefab;
    public Transform scrollView;
    public Transform scrollViewInitPanel;

    public GameObject failWindow;

    public void CheckingFileCount()
    {
        if(jsonSquares.Count == imageDatas.Count && jsonSquares.Count != 0)
        {
            InitPortrait();
        }
        else
        {
            failWindow.SetActive(true);
            Invoke("FailWindowSetActiveFalse", 3f);
            jsonSquares.Clear();
            imageDatas.Clear();
            foreach (Transform child in faceImage.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void FailWindowSetActiveFalse()
    {
        failWindow.SetActive(false);
    }
    public void Portrait()
    {
        for(int i = 0; i < imageDatas.Count; i++)
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
    public void RectanglesSetActiveFalse()
    {
        foreach(GameObjectList gameObjectList in jsonSquares)
        {
            for(int i = 0; i < gameObjectList.gameObjects.Count; i++)
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
    }


    public void ParseJSONData(string jsonData)
    {
        // Deserialize the JSON data into the JSONData object
        JSONData data = JsonUtility.FromJson<JSONData>(jsonData);

        // Access the "id" values
        int[] idArray = data.id;
        string[] regionNames = data.region_name;

        // assign
        for (int i = 0; i < idArray.Length ; i++)  
        {
            infoArray[i] = new Info();
            infoArray[i].id = idArray[i].ToString();
            infoArray[i].region_name = regionNames[i];
            infoArray[i].point = new List<int>();
        }

        // json 4 point split
        int start = jsonData.IndexOf("[[[");

        if (start != -1)
        {
            int end = jsonData.IndexOf("]]]", start);

            if (end != -1)
            {
                string bboxPointData = jsonData.Substring(start, end - start + 3);

                string[] bboxPoints = bboxPointData.Split(new string[] { "], [" }, System.StringSplitOptions.None);

                
                int idx = 0; // using 2 point to 4 point 
                foreach (string pointData in bboxPoints)
                {
                    string[] coordinates = pointData.Replace("[", string.Empty).Replace("]", string.Empty).Split(',');
                    if (idx % 2 == 0)
                    {
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[0]));
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[1]));
                    }
                    else
                    {
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[0]));
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[1]));
                    }
                    idx++;
                }
            }
        }

        // all Done parsing json data call ObjInstantManager function
        ObjInstantGameObject.GetComponent<ObjInstantManager>().ObjInstant(infoArray);
        // ObjInstant Function in ObjInstantManager Class in ObjInstantGameObject
    }
}
