using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SquareDataWrapper
{
    public List<SquareData> squares;
}

[System.Serializable]
public class SquareData
{
    public string id;
    public PointData point1;
    public PointData point2;
}

[System.Serializable]
public class PointData
{
    public float x;
    public float y;
}

public class JsonParsing : MonoBehaviour
{
    public List<SquareData> squares = null;

    public void ParseJSONData(string jsonData)
    {
        // Deserialize the JSON data into the SquareDataWrapper object
        SquareDataWrapper dataWrapper = JsonUtility.FromJson<SquareDataWrapper>(jsonData);

        // Clear existing squares list
        squares.Clear();

        // Add the parsed squares to the list
        squares.AddRange(dataWrapper.squares);

        // Access individual square objects and their properties
        if (squares != null)
        {
            foreach (SquareData square in squares)
            {
                Debug.Log("Square ID: " + square.id);
                Debug.Log("Point 1: (" + square.point1.x + ", " + square.point1.y + ")");
                Debug.Log("Point 2: (" + square.point2.x + ", " + square.point2.y + ")");
            }
        }
    }
}