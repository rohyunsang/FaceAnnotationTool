using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject InitPanel;
    public GameObject OptionPanel;

    public void OnInitPanel()
    {
        InitPanel.SetActive(true);
    }

    public void OnOptionPanel()
    {
        OptionPanel.SetActive(true);
    }

    public void OffInitPanel()
    {
        InitPanel.SetActive(false);
    }

    public void OffOptionPanel()
    {
        OptionPanel.SetActive(false);
    }
    

    
}
