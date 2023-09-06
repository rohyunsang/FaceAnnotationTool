using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnNameManager : MonoBehaviour
{
    
    public void BtnNamePassing(string name)
    {
        GameObject faceImage = GameObject.Find("faceImage");
        RectangleCreator rectangleCreator = faceImage.GetComponent<RectangleCreator>();

        if (rectangleCreator != null)
        {
            rectangleCreator.curObjName = name;
        }
        else
        {
            Debug.LogError("Failed to find or access RectangleCreator component.");
        }
        
    }
}
