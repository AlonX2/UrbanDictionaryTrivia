using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

[System.Serializable]
public class Data 
{
    public int Tokens;
    public int[] Strikes = new int[MenuScript.TypeIndex.Length];
    public bool MegaGay;
    public bool[] BoughtButtons;
    public DateTime[] LockedTime = new DateTime[50];
    public Global[] SavedQuestions = new Global[MenuScript.TypeIndex.Length];
    public static Data PlayerData = new Data();
   // public static bool Loaded = false;

    /*
    private void Awake()
    {
        PlayerData = this;
    }
    */


}
