using UnityEngine;
using System.Collections;
using System.IO;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FileBrowserTest : MonoBehaviour
{
    public Dictionary<string, string> jsonStrings = new Dictionary<string, string>(); // fileName, fileBytes

    public GameObject jsonManager;

    public string filePath = "";

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

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                string extension = Path.GetExtension(FileBrowser.Result[i]);

                if (extension == "")
                {
                    // 모든 .json 파일 처리
                    List<string> jsonFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.json");
                    foreach (string jsonFile in jsonFiles)
                    {
                        Debug.Log("Processing JSON file: " + Path.GetFileName(jsonFile));  // JSON 파일 이름 디버그
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jsonFile);
                        jsonStrings[Path.GetFileName(jsonFile)] = System.Text.Encoding.UTF8.GetString(bytes);

                    }

                    // 모든 .jpg 파일 처리
                    List<string> jpgFiles = GetAllFilesInDirectory(FileBrowser.Result[i], "*.jpg");
                    foreach (string jpgFile in jpgFiles)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);
                        jsonManager.GetComponent<JsonParsing>().MakeImageStringArray(bytes);

                        // .jpg 파일의 현재 디렉토리를 가져옵니다.
                        string currentDirectory = Path.GetDirectoryName(jpgFile);
                        filePath = Path.GetDirectoryName(jpgFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                    }
                }
                var ordered = jsonStrings.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                jsonStrings = ordered;
                foreach (KeyValuePair<string, string> entry in jsonStrings)
                {
                    Debug.Log("Key: " + entry.Key);
                }
                // 정렬된 딕셔너리에서 마지막 원소의 값을 가져옵니다.
                string lastJsonValue = jsonStrings.LastOrDefault().Value;

                // 마지막 원소의 값을 MakeJsonArray 메서드에 전달합니다.
                jsonManager.GetComponent<JsonParsing>().MakeJsonArray(lastJsonValue);
                jsonManager.GetComponent<JsonParsing>().CheckingFileCount();
                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[i]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[i], destinationPath);
            }
        }
    }

    public List<string> GetAllFilesInDirectory(string directoryPath, string searchPattern)
    {
        List<string> files = new List<string>();

        // 현재 디렉토리에서 파일들을 가져옵니다.
        files.AddRange(Directory.GetFiles(directoryPath, searchPattern));

        // 모든 하위 디렉토리를 가져와 각 하위 디렉토리에 대해 재귀적으로 이 함수를 호출합니다.
        foreach (string subDirectory in Directory.GetDirectories(directoryPath))
        {
            files.AddRange(GetAllFilesInDirectory(subDirectory, searchPattern));
        }

        return files;
    }
}