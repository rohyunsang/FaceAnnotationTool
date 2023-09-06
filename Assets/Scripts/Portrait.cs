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

        string currentId = transform.name; // 여기서 transform.name이 해당 포트레이트의 id 값과 일치한다고 가정합니다.

        int idx = GetIndexById(currentId);

        if (idx != -1)  // 유효한 인덱스인 경우
        {
            Debug.Log(idx);
            JsonParsingManagerObj.GetComponent<JsonParsing>().QueueManager(idx);
        }
    }

    private int GetIndexById(string id)
    {
        List<Info> parsedInfo = JsonParsingManagerObj.GetComponent<JsonParsing>().parsedInfo;

        for (int i = 0; i < parsedInfo.Count; i++)
        {
            if (parsedInfo[i].id == id)
            {
                return i;  // 인덱스를 반환
            }
        }

        return -1;  // id를 찾지 못한 경우
    }
}