using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SaveLoad : MonoBehaviour
{
    bool FirstPause = true;
    private void Awake()
    {
        /*
        var settings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };
        */
        

    }
    private void Start()
    {

            if (bool.Parse(PlayerPrefs.GetString("Saved", "false")))
            {
             
                StreamReader sr = new StreamReader(Application.persistentDataPath + @"/SaveFiles/SaveData.json");
                string data = sr.ReadLine();
                Debug.Log(data);

                data = tools.Decrypt(data, "some code");
                Debug.Log(data);
                Debug.Log("here1");
                Data.PlayerData = JsonConvert.DeserializeObject<Data>(data);
                Data.PlayerData.Strikes = JsonConvert.DeserializeObject<Data>(data).Strikes;
                sr.Dispose();
                GameManager.Instance.MatchData();
                sr.Close();
              
        

            }
        else
            {
                GameManager.Instance.FirstWrite();
            }


    //    }   

        



    }
    private void OnApplicationPause(bool pause)
    {
        if (!FirstPause)
        {
            Debug.Log("pausing");
            Save();
            FirstPause = false;
        }
 

    }
    private void OnApplicationQuit()
    {
        Debug.Log("quitting");
        Save();
    }
  
    void Save()
    {
        PlayerPrefs.SetString("Saved", "true");
        if (!Directory.Exists(Application.persistentDataPath + @"/SaveFiles"))
            Directory.CreateDirectory(Application.persistentDataPath + @"/SaveFiles");
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + @"/SaveFiles/SaveData.json");
        string data = JsonConvert.SerializeObject(Data.PlayerData);
        Debug.Log(data);
        sw.WriteLine(tools.Encrypt(data, "some code"));
        sw.Close();

        GameInspector.AddToSavedQuestions(new Global(GlobalScript.Instance));
        //put it somewhere else? 
        Directory.Move(Application.persistentDataPath + @"/JsonFiles", Application.persistentDataPath + @"/JsonFilesPost");

    }


}
