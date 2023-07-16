using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

public class JsonParsing : MonoBehaviour
{
    [SerializeField]
    private Info[] infoArray = new Info[8];
    public void ParseJSONData(string jsonData)
    {
        // Deserialize the JSON data into the JSONData object
        JSONData data = JsonUtility.FromJson<JSONData>(jsonData);

        // Access the "id" values
        int[] idArray = data.id;
        string[] regionNames = data.region_name;
        

        // Print the "id" values
        
        for(int i = 0; i < idArray.Length ; i++)
        {
            infoArray[i] = new Info();
            infoArray[i].id = idArray[i].ToString();
            infoArray[i].region_name = regionNames[i];
            infoArray[i].point = new List<int>();
        }

        int start = jsonData.IndexOf("[[[");

        if (start != -1)
        {
            int end = jsonData.IndexOf("]]]", start);

            if (end != -1)
            {
                string bboxPointData = jsonData.Substring(start, end - start + 3);

                string[] bboxPoints = bboxPointData.Split(new string[] { "], [" }, System.StringSplitOptions.None);

                Debug.Log("BBox Point List:");
                int idx = 0; // using 2 point to 4 point 
                foreach (string pointData in bboxPoints)
                {
                    string[] coordinates = pointData.Replace("[", string.Empty).Replace("]", string.Empty).Split(',');
                    if (idx % 2 == 0)
                    {
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[0]));
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[1]));
                        Debug.Log(idx / 2);
                    }
                    else
                    {
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[0]));
                        infoArray[idx / 2].point.Add(int.Parse(coordinates[1]));
                        Debug.Log(idx / 2 + " idx");
                    }
                    idx++;
                }
            }
        }

        //
        for(int i = 0; i < infoArray.Length; i++)
        {
            Debug.Log(infoArray[i].id);
            Debug.Log(infoArray[i].region_name);
            foreach(int a in infoArray[i].point)
            {
                Debug.Log(a);
            }
            Debug.Log(""); // Add an empty line between each Info object
        }
    }
}
