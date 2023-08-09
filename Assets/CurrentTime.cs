using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTime : MonoBehaviour
{
    public string currentTime;

    public void CurrentTimeBtn()
    {
        DateTime localDateTime = DateTime.Now;
        currentTime = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log(currentTime);
    }
}