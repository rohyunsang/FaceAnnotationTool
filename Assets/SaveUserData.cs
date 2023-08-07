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

    public GameObject UserDataCheckingImage;

    public void OnLoginBtn()
    {
        idCheckText.text += idField.text;
        emailCheckText.text += emailField.text;
    }
    public void OnUserDataCheckImage()
    {
        UserDataCheckingImage.SetActive(true);
    }
    public void OffUserDataCheckImage()
    {
        UserDataCheckingImage.SetActive(false);
    }
    public void DeleteUserData()
    {
        idCheckText.text = "아이디 : ";
        emailCheckText.text = "이메일 : ";
    }
}
