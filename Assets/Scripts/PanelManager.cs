using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour  // panel and info controller
{
    public GameObject InitPanel;
    public GameObject OptionPanel;
    public GameObject SavePanel;
    public GameObject LoginPanel;

    public GameObject faceField;

    public GameObject ImageClickInfo;
    public GameObject ImageEditBtn;

    public GameObject selectModeInfo;

    public GameObject savejsonBtn;
    public GameObject jsonExportBtn;
    public GameObject saveFaceLineBtn;
    public GameObject jsonCircleExportBtn;

    public void OnClickBBOXModeBtn()
    {
        jsonCircleExportBtn.SetActive(false);
        savejsonBtn.SetActive(true);
        jsonExportBtn.SetActive(true);
        saveFaceLineBtn.SetActive(false);
    }
    
    public void OnClickFaceLineBtn()
    {
        jsonCircleExportBtn.SetActive(true);
        savejsonBtn.SetActive(false);
        jsonExportBtn.SetActive(false);
        saveFaceLineBtn.SetActive(true);
    }

    public void OnSelectModeInfo()
    {
        selectModeInfo.SetActive(true);
    }
    public void OffSelectModeInfo()
    {
        selectModeInfo.SetActive(false);
    }

    public void OnImageEditBtn()
    {
        ImageEditBtn.SetActive(true);
    }
    public void OffImageEditBtn()
    {
        ImageEditBtn.SetActive(false);
    }

    public void OffImageClickInfo()
    {
        Invoke("InvokeOffImageClickInfo", 3f);
    }

    public void InvokeOffImageClickInfo()
    {
        ImageClickInfo.SetActive(false);
    }
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

    public void OnLoginPanel()
    {
        LoginPanel.SetActive(true);
    }
    public void OffLoginPanel()
    {
        LoginPanel.SetActive(false);
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
