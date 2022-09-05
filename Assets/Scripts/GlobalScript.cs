using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class GlobalScript : MonoBehaviour
{
    #region dontTouch
    #region DontDestroyOnLoad
    public static GlobalScript Instance;
    public string filter;
    private void Awake()
    { 
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public int rightIndex;
    public Entery[] options = new Entery[4];
    public Entery chosenEntery;
    public void LoadNewQ(string category)
    {
        chosenEntery = GetChosenEntery(category);
        rightIndex = tools.RandomRange(0,options.Length);
        Entery[] other3 = Get3Similar(chosenEntery);
        options[rightIndex] = chosenEntery;
        int other3In = 0;
        for(int i = 0; i < options.Length; i++)
        {
            if (rightIndex != i)
                options[i] = other3[other3In];
            else
                other3In--;
            other3In++;
        }
        UsedEnterys.addToUsed(chosenEntery);
        Debug.Log(rightIndex+1);
        Debug.Log(GameInspector.SavedQuestions);
    }
    public void LoadQ(Global g, string category)
    {
        if (g == null)
        {
            LoadNewQ(category);
            return;
        }
        rightIndex = g.rightIndex;
        options = g.options;
        chosenEntery = g.chosenEntery;
        filter = g.filter;
        Debug.Log(rightIndex+1);
    }
    /*
    public void DisplayButtons()
    {

    }
    */
    #region Generator
    //public string filter;
    private void Start()
    {
    }
    public static Entery GetChosenEntery(string category)
    {
        int linesCount = tools.NumOfLinesInFile(category);
        int randomLineNum = tools.RandomRange(0,linesCount-1);
        Entery randomEntery = new Entery(category, randomLineNum);
        randomEntery = randomEntery.GetEnteryInCategoryInLineRemove();//contains the random entery
        return randomEntery;
    }
    public static Entery[] Get3Similar(Entery chosenEntery)
    {
        Entery[] retEnterys = new Entery[3];
        string[] exceptOf = { chosenEntery.lowercase_word, string.Empty, string.Empty };
        int linesCount = tools.NumOfLinesInFile(chosenEntery.category);

        Entery ent;
        for (int i = 0; i < retEnterys.Length; i++)
        {
            ent = new Entery(chosenEntery.category, tools.RandomRange(0, linesCount-1));
            ent = ent.GetEnteryInCategoryInLine();
            while (tools.IsIn(ent.lowercase_word, exceptOf))
            {
                ent = new Entery(chosenEntery.category, tools.RandomRange(0, linesCount-1));
                ent = ent.GetEnteryInCategoryInLine();
            }
            retEnterys[i] = ent;
            if (i == 2)
                break;
            exceptOf[i + 1] = ent.lowercase_word;
        }
        return retEnterys;
    }
    #endregion
    #endregion
    
    #region constructor
    
    public GlobalScript()
    {
        this.filter = null;
        this.chosenEntery = null;
        this.options = new Entery[4];
        this.rightIndex = 0;
    }

    #endregion
    
}
