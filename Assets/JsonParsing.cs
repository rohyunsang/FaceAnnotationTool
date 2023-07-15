using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class JSONData
{
    public int[] id;
    public string[] region_name;
}

public class JsonParsing : MonoBehaviour
{
    public void ParseJSONData(string jsonData)
    {
        // Deserialize the JSON data into the JSONData object
        JSONData data = JsonUtility.FromJson<JSONData>(jsonData);

        // Access the "id" values
        int[] idArray = data.id;
        string[] regionNames = data.region_name;
        

        // Print the "id" values
        Debug.Log("id:");
        foreach (int id in idArray)
        {
            Debug.Log(id);
        }

        // Print the "region_name" values
        Debug.Log("region_name:");
        foreach (string regionName in regionNames)
        {
            Debug.Log(regionName);
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

                foreach (string pointData in bboxPoints)
                {
                    string[] coordinates = pointData.Replace("[", string.Empty).Replace("]", string.Empty).Split(',');

                    int x = int.Parse(coordinates[0]);
                    int y = int.Parse(coordinates[1]);

                    Debug.Log("[" + x + ", " + y + "]");
                }
            }
        }

    }
}
