using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

public class UsedEnterys : MonoBehaviour
{
    public static void addToUsed(Entery thisEntery)
    {
        thisEntery = GlobalScript.Instance.chosenEntery;
        StreamWriter sw1 = new StreamWriter(Entery.persistentDataPath + @"/JsonFiles/UsedEnterys.json", append: true);
        sw1.WriteLine(JsonUtility.ToJson(thisEntery));
        sw1.Close();
        tools.UpdateNumOfLinesInFiles(thisEntery.category);
    }
    public static void EmptyUsedEnterys()
    {
        StreamReader sr = new StreamReader(Entery.persistentDataPath + @"/JsonFiles/UsedEnterys.json");
        string line;
        while ((line = sr.ReadLine())!=null)
        {
            Entery enteryInLine = JsonUtility.FromJson<Entery>(line);
            if (enteryInLine != null)
            {
                using (StreamWriter sw = new StreamWriter(Entery.persistentDataPath + @"/JsonFiles/" + enteryInLine.category + ".json", append: true))
                {
                    sw.WriteLine(JsonUtility.ToJson(enteryInLine.Toentry()));
                }
            }
        }
        sr.Close();
        File.WriteAllText(Entery.persistentDataPath + @"/JsonFiles/UsedEnterys.json", string.Empty);
    }
    

}