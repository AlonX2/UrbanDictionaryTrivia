using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;


public class GameInspector : MonoBehaviour
{
    public static GameInspector currentGameInspector;
    private void Awake()
    {
        currentGameInspector = this;
        if (SceneManager.GetActiveScene().buildIndex != 0 && GameManager.Instance == null)
        {
            SceneManager.LoadScene(0);
        }
        if (!Directory.Exists(Application.persistentDataPath + @"/JsonFilesPost"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + @"/JsonFiles");
            string[] typeIndex = MenuScript.TypeIndex;
            TextAsset JsonFile;
            for (int i = 0; i < typeIndex.Length; i++)
            {
                JsonFile = Resources.Load<TextAsset>(@"JsonFiles/" + typeIndex[i] + "Filter");
                File.WriteAllText(Application.persistentDataPath + @"/JsonFiles/" + typeIndex[i] + "Filter.json", JsonFile.text);

            }
            JsonFile = Resources.Load<TextAsset>(@"JsonFiles/UsedEnterys");
            File.WriteAllText(Application.persistentDataPath + @"/JsonFiles/UsedEnterys.json", JsonFile.text);
            File.Create(Application.persistentDataPath + @"/JsonFiles/SavedQuestions.json");
            Notification.GetNotificationsWritenInFile();
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + @"/JsonFiles");
            string[] typeIndex = MenuScript.TypeIndex;
            for (int i = 0; i < typeIndex.Length; i++)
            {

                File.Copy(Application.persistentDataPath + @"/JsonFilesPost/" + typeIndex[i] + "Filter.json", Application.persistentDataPath + @"/JsonFiles/" + typeIndex[i] + "Filter.json");

            }
            File.Copy(Application.persistentDataPath + @"/JsonFilesPost/UsedEnterys.json", Application.persistentDataPath + @"/JsonFiles/UsedEnterys.json");
            File.Copy(Application.persistentDataPath + @"/JsonFilesPost/SavedQuestions.json", Application.persistentDataPath + @"/JsonFiles/SavedQuestions.json");
            tools.DeleteDirectory(Application.persistentDataPath + @"/JsonFilesPost");
         //   Directory.Move(Application.persistentDataPath + @"/JsonFilesTemp", Application.persistentDataPath + @"/JsonFiles");

        }
    }

    public static Global[] SavedQuestions = Data.PlayerData.SavedQuestions;
    public static Global GetFromSavedInCategory(string category)
    {
        SavedQuestions = Data.PlayerData.SavedQuestions;
        return SavedQuestions[MenuScript.CurrentIndex];
    }

    public static void AddToSavedQuestions(Global g)
    {
        SavedQuestions[MenuScript.CurrentIndex] = new Global(g.filter, g.chosenEntery, g.options, g.rightIndex);
        Data.PlayerData.SavedQuestions = SavedQuestions;

    }

    public static int CategoryToIndex(string filter)
    {
        int i = 0;
        for (i = 0; i < MenuScript.TypeIndex.Length; i++)
        {
            if (MenuScript.TypeIndex[i] + "Filter" == filter)
                return i;
        }
        return i;
    }

    private void Start()
    {
        IsConnectedToInternet();
    }
    public GameObject NetworkDownPanel;
    public bool IsConnectedToInternet()
    {
        if (Data.PlayerData.MegaGay)
            return true;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NetworkDownPanel.SetActive(true);
            return false;
        }
        return true;
    }
    public void OnX()
    {
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false); //eventsystem => pressedbutton => parent => disable
    }


}