using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject InitPanel;
    public GameObject OptionPanel;
    public GameObject SavePanel;
    public GameObject faceField;

    public void OnInitPanel()
    {
        InitPanel.SetActive(true);
    }

    public void OnOptionPanel()
    {
        OptionPanel.SetActive(true);
    }
    public void OnSavePanel()
    {
        SavePanel.SetActive(true);
    }

    public void OffSavePanel()
    {
        SavePanel.SetActive(false);
    }

    public void OffInitPanel()
    {
        InitPanel.SetActive(false);
    }

    public void OffOptionPanel()
    {
        OptionPanel.SetActive(false);
    }
    
    public void DeleteObjects()
    {
        foreach (Transform child in faceField.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
