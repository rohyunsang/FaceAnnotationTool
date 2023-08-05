using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public GameObject JsonParsingManagerObj;
    public GameObject checkingImage;
    public void PortraitClick()
    {
        JsonParsingManagerObj = GameObject.Find("JsonParsingManager");
        JsonParsingManagerObj.GetComponent<JsonParsing>().QueueManager(int.Parse(transform.name));
    }
    // need data delete json manager . parsing 한것으로 
}
