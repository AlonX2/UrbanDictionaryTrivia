using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class Entery : entry
{
    public static string persistentDataPath;
    public Entery(string category, int line)
    {
        this.category = category;
        this.line = line;
    }
    public string category;
    public int line;

    public Entery GetEnteryInCategoryInLineRemove()
    {
        Entery retEntery = new Entery(this.category, this.line);

        string tempFile = persistentDataPath + @"/JsonFiles/Temp.json";
        using (StreamReader sr = new StreamReader(persistentDataPath + @"/JsonFiles/" + this.category + ".json"))
        using (StreamWriter sw = new StreamWriter(tempFile,append: true))
        {
            for (int i = 0; i < this.line-1; i++)
            {
                sw.WriteLine(sr.ReadLine());
            }
            string linestr = sr.ReadLine(); //now contains entery line string
            retEntery.entToEnt(JsonUtility.FromJson<entry>(linestr));
            string line;
            while((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
        }
        File.Delete(persistentDataPath + @"/JsonFiles/" + this.category + ".json");
        File.Move(tempFile, persistentDataPath + @"/JsonFiles/" + this.category + ".json");
        return retEntery;
    }
    public Entery GetEnteryInCategoryInLine()
    {
        Entery retEntery = new Entery(this.category, this.line);
        StreamReader file = new StreamReader(persistentDataPath + @"/JsonFiles/" + this.category + ".json");
        for (int i = 0; i < this.line; i++)
        {
            file.ReadLine();
        }
        string linestr = file.ReadLine(); //now contains entery line string
        file.Close();
        retEntery.entToEnt(JsonUtility.FromJson<entry>(linestr));
        return retEntery;
    }
    public override string ToString()
    {
        return this.word + ", line number: " + this.line+", category: "+this.category+ ", definition: "+this.definition;
    }
    public void entToEnt(entry ent) //there has got to be a better way but this is pretty efficent 
    {
        this.definition = ent.definition;
        this.word = ent.word;
        this.example = ent.example;
        this.lowercase_word = ent.lowercase_word;
    }
    public entry Toentry()
    {
        entry ent = new entry();
        ent.definition = this.definition;
        ent.word = this.word;
        ent.example = this.example;
        ent.lowercase_word = this.lowercase_word;
        return ent;
    }

}
