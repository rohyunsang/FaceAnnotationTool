using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveUserData : MonoBehaviour
{
    public InputField idField;
    public InputField emailField;

    public Text idCheckText;
    public Text emailCheckText;
    public Text idCheckTextOptionPanel;
    public Text emailCheckTextOptionPanel;

    public GameObject UserDataCheckingImage;

    public void OnLoginBtn()
    {
        idCheckText.text += idField.text;
        emailCheckText.text += emailField.text;
        idCheckTextOptionPanel.text += idField.text;
        emailCheckTextOptionPanel.text += emailField.text;
    }
    public void OnUserDataCheckImage()
    {
        if (!idField.text.Equals("") && !emailField.text.Equals(""))
            UserDataCheckingImage.SetActive(true);
    }
    public void OffUserDataCheckImage()
    {
        UserDataCheckingImage.SetActive(false);
    }
    public void DeleteUserData()
    {
        idCheckText.text = "이름 : ";
        emailCheckText.text = "이메일 : ";
        idCheckTextOptionPanel.text = "이름 : ";
        emailCheckTextOptionPanel.text = "이메일 : ";
    }
}