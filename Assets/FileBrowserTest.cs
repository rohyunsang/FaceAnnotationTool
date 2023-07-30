using UnityEngine;
using System.Collections;
using System.IO;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.UI;

public class FileBrowserTest : MonoBehaviour
{
    public string jsonString = "";
    public RawImage faceImage;
    public GameObject jsonManager;

    void Start()
    {
    }

    public void ShowFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Files", ".jpg", ".png", ".json"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".json");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                Debug.Log(FileBrowser.Result[i]);

                string extension = Path.GetExtension(FileBrowser.Result[i]);

                if (extension == "")
                {
                    string[] filesInDirectory = Directory.GetFiles(FileBrowser.Result[i], "*.json");
                    foreach (string file in filesInDirectory)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(file);
                        jsonString = System.Text.Encoding.UTF8.GetString(bytes);
                        jsonManager.GetComponent<JsonParsing>().MakeJsonArray(jsonString);
                    }
                }
                else if (extension.ToLower() == ".jpg")  //jpg file
                {
                    byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[i]);

                    // Create a Texture2D from the image bytes
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(bytes);

                    faceImage.texture = texture;
                }
                else if (extension.ToLower() == ".json")
                {
                    byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[i]);
                    jsonString = System.Text.Encoding.UTF8.GetString(bytes);
                    jsonManager.GetComponent<JsonParsing>().MakeJsonArray(jsonString);
                }

                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
            }
        }
    }
}