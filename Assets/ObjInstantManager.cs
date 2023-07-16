using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantManager : MonoBehaviour
{
    public void ObjInstant(Info[] infoArray)
    {
        // using Debug
        for (int i = 0; i < infoArray.Length; i++)
        {
            
            Debug.Log(infoArray[i].id + " Obj");
            Debug.Log(infoArray[i].region_name);
            foreach (int a in infoArray[i].point)
            {
                Debug.Log(a);
            }
            Debug.Log(""); // Add an empty line between each Info object
        }
    }
}
