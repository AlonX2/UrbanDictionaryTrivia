using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public CanvasScript CanvasScript;
    public int delay;
    Stopwatch stopwatch = new Stopwatch();
    GameManager gameManager;
    GlobalScript GlobalScript;
    GameObject Canvas;
    //SwipeManager SwipeManager;
    Image[] ImageArray;
    public GameObject[] ButtonArray;//if changing num of buttons change here!


    EventSystem eventSystem;
    public static ButtonManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        eventSystem = EventSystem.current;

        //SwipeManager = GameObject.Find("SwipeManager").GetComponent<SwipeManager>();
        // SwipeManager.GetButtonManager(gameObject);
    }
    static Color[] ogColors;
    private void Start()
    {
        if (gameManager != null)
            return;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GlobalScript = GameObject.Find("GlobalManager").GetComponent<GlobalScript>();
        Canvas = GameObject.Find("OptionsCanvas");
        ImageArray = Canvas.GetComponentsInChildren<Image>();
        ButtonArray = new GameObject[ImageArray.Length];
        for (int i = 0; i < ImageArray.Length; i++)
        {
            ButtonArray[i] = ImageArray[i].gameObject;
        }
        ogColors = new Color[ImageArray.Length];
        for (int i = 0; i < ImageArray.Length; i++)
        {
            ogColors[i] = ImageArray[i].color;
        }
    }
    public void UpdateCanvas(GameObject newCanvas)
    {
        Canvas = newCanvas;
        ImageArray = Canvas.GetComponentsInChildren<Image>();
        for (int i = 0; i < ImageArray.Length; i++)
        {
            ButtonArray[i] = ImageArray[i].gameObject;
            ImageArray[i].color = ogColors[i];
        }
    }
    // Update is called once per frame
    void Update()
    {/*
        if(stopwatch.ElapsedMilliseconds >= delay)
        {
            stopwatch.Reset();
            CanvasManager.LoadNewPage();

        }*/

    }
    public void ActivateEventSystem()
    {
        eventSystem.gameObject.SetActive(true);
    }
    public static void ButtonDisplayer(GameObject optionsCanvas)
    {
        Text[] buttonsTexts = optionsCanvas.GetComponentsInChildren<Text>();
        for (int i = 0; i < buttonsTexts.Length; i++)
        {
            buttonsTexts[i].text = GlobalScript.Instance.options[i].word;
            if (TextDisplayer.IsOverflowing(buttonsTexts[i]))
                buttonsTexts[i].resizeTextForBestFit = true;
        }
    }
    public void ReDisplay()
    {
        Awake();
    }
    #region ButtonPress
    public void OnButtonPress()
    {
        if (!GameInspector.currentGameInspector.IsConnectedToInternet())
            return;
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
        //SwipeManager.buttonPressed = true; 
        if (GlobalScript.chosenEntery.word != null && PressedButton.GetComponentInChildren<Text>().text == GlobalScript.chosenEntery.word)//is the text on the button = correct guess name?
        {
            //correct stuff 
            TokenSystem.AddTokensToCoin(1);
        }
        else
        {
            gameManager.StrikeCall();
        }
        CheckForLeftDefinitions(MenuScript.CurrentIndex);
        if (gameManager.Strikes[gameManager.index] == 5)
            return;
        if (tools.numOfLinesInFiles[MenuScript.CurrentIndex] <= 5 && tools.numOfLinesInFiles[MenuScript.CurrentIndex] != default)
            return;
        EndScene();
    }
    public async void OnSkipButtonPress()
    {
        if (!GameInspector.currentGameInspector.IsConnectedToInternet())
            return;
        bool watched;
        if (Data.PlayerData.Premium)
        {
            watched = true;
        }
        else
        {
            Task<bool> Ad = GameManager.Instance.ShowRewardedAd();
            await Ad;
            watched = Ad.Result;
            UnityEngine.Debug.Log(watched);
        }
        if (watched)
        {
            //handle if watched
            GameManager.Instance.StrikeMinus();
            GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
            EndScene();
        }

    }
    public async void EndScene()
    {

        for (int i = 0; i < ButtonArray.Length; i++)
        {
            if (ButtonArray[i].GetComponentInChildren<Text>().text == GlobalScript.options[GlobalScript.rightIndex].word)//is the text on the button = correct guess name?
            {
                //  ColorChanger(true, ButtonArray[i].GetComponent<Image>());
                ButtonArray[i].GetComponent<Image>().color = new Color(0.0431f, 0.4811f, 0.0716f, ButtonArray[i].GetComponent<Image>().color.a);



            }
            else
                ButtonArray[i].GetComponent<Image>().color = new Color(0.4150f, 0.0097f, 0.0179f, ButtonArray[i].GetComponent<Image>().color.a);

            // ColorChanger(false, ButtonArray[i].GetComponent<Image>());
        }
        eventSystem.gameObject.SetActive(false);
        await LoadNewScene();
    }
    public static async Task LoadNewScene()
    {
    

        Task PageGettingReady = Task.Run(() => CanvasManager.GetReadyToLoadNewPage());
        Task Delay = Task.Delay(Instance.delay);
        await PageGettingReady;
        await Delay;
        try { CanvasManager.LoadNewPage(); }
        catch (Exception error)
        {
            if (!error.GetType().IsAssignableFrom(typeof(MissingReferenceException)))
                throw;
        }

    }
    public void OnBackToMenuPress()
    {
        GameInspector.AddToSavedQuestions(new Global(GlobalScript.Instance));
        GameManager.Instance.SceneChange("MainMenu");
    }
    public static void ColorChanger(bool MoreVibrent, Image Imege)
    {
    }
    async void CheckForLeftDefinitions(int index)
    {
        UnityEngine.Debug.Log(tools.numOfLinesInFiles[index]);
        if ( tools.numOfLinesInFiles[index] <= 5 && tools.numOfLinesInFiles[index] != default)
        {
            //   GameManager.Instance.SceneChange("MainMenu");
            var loading = SceneManager.LoadSceneAsync("MainMenu");
            while (!loading.isDone)
            {
                await Task.Delay(25);
            }
            MenuScript.Instance.OnOutOfDef();
            // out of definitions
        }
    }

    #endregion ButtonPress

}

