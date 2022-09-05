using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class MenuScript : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject LockedPanel;
    public GameObject shopPanel;
    public GameObject confirmPanel;
    public GameObject PayButton;
    public GameObject OutOfDefPanel;
    public int PaidIndexThreshold;
    public bool inPanel = false;
    public static string[] TypeIndex = { "Memes", "TV", "'Merica",  "VideoGames", "Politics", "SchoolLife", "Rock", "Metal", "Rap", "Food",  "WWII", "Apple", "BigCompanies", "SchoolSubjects", "Sports", "Relationships", "Anime", "Creepy", "FamousMovies", "Movies", "Rock&Metal[full]" };
    public static int CurrentIndex = 0;
    Text menuText;
    bool Added60;
    public TextMeshProUGUI TimeText;
    int[] Price = {10,20,30,35,45,45,45,45,85,85,85,15,45,45,45,85,98,117,150,150,225,265,300 };
    GameManager GameManager;
    GlobalScript GlobalScript;
    public static MenuScript Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        menuText = GameObject.Find("Text").GetComponent<Text>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        menuText.text = TypeIndex[CurrentIndex];

        if (GameManager.Strikes[CurrentIndex] >= 5)
        {
            menuText.color = Color.gray;

        }


    }
    public void CheckStrikes()
    {
        if (GameManager.Strikes[CurrentIndex] >= 5)
        {
            menuText.color = Color.gray;

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (LockedPanel != null && LockedPanel.activeInHierarchy)
        {
            TimeSpan TimePassed = DateTime.Now - GameManager.LockTime[CurrentIndex];
            TimeText.text = (60 - TimePassed.Minutes) + "m";
            if (TimeText.text == "0m")
            {
                GameManager.Strikes[CurrentIndex] = 0;
                menuText.color = Color.white;
                OnLockedX();
            }


        }


    }
    #region MainCanvasButtons
    public async void OnButtonPress()
    {

        if (inPanel)
            return;
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;


        if (PressedButton.name == "RightButton")
        {
            if (CurrentIndex + 1 < TypeIndex.Length)
                CurrentIndex++;
            else
                CurrentIndex = 0;
            if (GameManager.Strikes[CurrentIndex] >= 5)
            {
                menuText.color = Color.gray;

            }
            else
                menuText.color = Color.white;


        }
        if (PressedButton.name == "LeftButton")
        {
            if (CurrentIndex - 1 >= 0)
                CurrentIndex--;
            else
                CurrentIndex = TypeIndex.Length - 1;
            if (GameManager.Strikes[CurrentIndex] >= 5)
            {
                menuText.color = Color.gray;

            }
            else
                menuText.color = Color.white;

        }
        if (GameManager.LockTime[CurrentIndex] != default)
        {
            TimeSpan TimePassed = DateTime.Now - GameManager.LockTime[CurrentIndex];
            if ((TimePassed.Minutes >= 60))
            {
                GameManager.Strikes[CurrentIndex] = 0;
                menuText.color = Color.white;
                GameManager.LockTime[CurrentIndex] = default;
            }

        }


        if (PressedButton.name == "StartButton")
        {
            if (tools.numOfLinesInFiles[CurrentIndex] <= 5 && tools.numOfLinesInFiles[CurrentIndex] != default)
            {
              Instance.OnOutOfDef();
                return;
                // out of definitions
            }
            if (GameManager.Strikes[CurrentIndex] >= 5)
            {
                OnLockedActive();
            }
            else
            {
                if (!GameInspector.currentGameInspector.IsConnectedToInternet())
                    return;
                await LoadingScript.LoadWhileTask(Task.Run(() => GameManager.SetFilter(TypeIndex[CurrentIndex], CurrentIndex)));
                GameManager.SceneChange("DefinitionScreen");
            }//


        }
        if (PressedButton.name == "SettingsButton")
        {
            settingsPanel.SetActive(true);
            inPanel = true;
        }
        if (PressedButton.name == "ShopButton")
        {
            TokenSystem.OnButtonPressIn();
        }
        if (CurrentIndex >= PaidIndexThreshold && !GameManager.BoughtButtons[CurrentIndex - PaidIndexThreshold])
        {
            PayButton.SetActive(true);
            PayButton.GetComponentInChildren<Text>().text = "UNLOCK FILTER FOR " + Price[CurrentIndex - PaidIndexThreshold] + " TOKENS";
        }
        else
        {
            PayButton.SetActive(false);

        }
        menuText.text = TypeIndex[CurrentIndex];
        LabelTextManager.ChangeText();
        //Debug.Log(TypeIndex[CurrentIndex]);




    }
    #endregion
    #region SettingsButtons
    public void OnSettingsX()
    {
        settingsPanel.SetActive(false);
        inPanel = false;
    }
    public void OnSettingsReset()
    {
        confirmPanel.SetActive(true);
    }
    #endregion
    #region ShopButtons
    public void OnXShopButtonPress()
    {
        shopPanel.SetActive(false);
        inPanel = false;
    }
    public void OnResetSettingsButtonPress()
    {
        confirmPanel.SetActive(true);
    }
    #endregion
    #region LockedPanel
    public void OnLockedX()
    {
        LockedPanel.SetActive(false);
        inPanel = false;
    }
    public void OnLockedActive()
    {
        inPanel = true;
        LockedPanel.SetActive(true);
    }
    /*
    public void OnLockedAd()
    {
        GameManager.Strikes[CurrentIndex] = 0;
        OnLockedX();
        menuText.color = Color.white;
    }
    */
    /*
    public void OnLockedToken()
    {
        GameManager.Strikes[CurrentIndex] = 0;
        OnLockedX();
        menuText.color = Color.white;
    }
    */
    public async void OnLockedAdPress()
    {
        Task<bool> Ad = GameManager.Instance.ShowRewardedAd();
        await Ad;
        bool watched = Ad.Result;
        UnityEngine.Debug.Log(watched);
        if (watched)
        {
            if (DateTime.Now - GameManager.LockTime[CurrentIndex] < new TimeSpan(0, 0, 40, 0))
                GameManager.Instance.LockTime[CurrentIndex] = GameManager.Instance.LockTime[CurrentIndex].Subtract(new TimeSpan(0, 0, 20, 0));
            else
            {
                GameManager.Strikes[CurrentIndex] = 0;
                menuText.color = Color.white;
                OnLockedX();
            }
        }
    }
    public void OnLockedPayPress()
    {
       if(TokenSystem.AddTokensToCoin(-5))
        {
            if (DateTime.Now - GameManager.LockTime[CurrentIndex] < new TimeSpan(0, 0, 40, 0))
                GameManager.Instance.LockTime[CurrentIndex] = GameManager.Instance.LockTime[CurrentIndex].Subtract(new TimeSpan(0, 0, 20, 0));
            else
            {
                GameManager.Strikes[CurrentIndex] = 0;
                menuText.color = Color.white;
                OnLockedX();
            }
        }

    }

    #endregion

    #region confirmPanelButtons
    public async void OnConfirmButtonPressYes()
    {
        await LoadingScript.LoadWhileTask(Task.Run(() => UsedEnterys.EmptyUsedEnterys()));
        for(int i=0; i< TypeIndex.Length; i++)
        {
            tools.numOfLinesInFiles[CurrentIndex] = default;
        }
        confirmPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inPanel = false;
    }
    public void OnConfirmButtonPressNo()
    {
        confirmPanel.SetActive(false);
        inPanel = false;
    }
    #endregion//remenber changing
    #region PayButton
    public void OnPayPress()
    {
        //remeber to change!!!!
        if (!TokenSystem.AddTokensToCoin(-50))
            return;
        //take funds or what not
        GameManager.BoughtButtons[CurrentIndex - PaidIndexThreshold] = true;
        Data.PlayerData.BoughtButtons = GameManager.BoughtButtons;
        PayButton.SetActive(false);




    }

    #endregion
    #region OutOfDef
    public void OnOutOfDef()
    {
        OutOfDefPanel.SetActive(true);
    }
    public void OnOutOfDefSettings()
    {
        OnOutOfDefX();
        settingsPanel.SetActive(true);
        inPanel = true;

    }
    public void OnOutOfDefX()
    {
        OutOfDefPanel.SetActive(false);

    }
    #endregion

}



