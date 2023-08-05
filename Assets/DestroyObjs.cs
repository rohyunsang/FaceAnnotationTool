using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DestroyObjs : MonoBehaviour
{
    public GameObject faceImage;
    public GameObject jsonManager; //square,image.
    public GameObject mainScrollViewContent;
    public GameObject initScrollViewContent;

    public void DestroyBtn()
    {
        // faceImage의 자식 오브젝트 모두 삭제

        jsonManager.GetComponent<JsonParsing>().ClearObjs();

        // mainScrollViewContent의 자식 오브젝트 모두 삭제
        foreach (Transform child in mainScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        // initScrollViewContent의 자식 오브젝트 모두 삭제
        foreach (Transform child in initScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
