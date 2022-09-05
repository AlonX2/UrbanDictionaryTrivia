using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ExampleManager : MonoBehaviour
{
    public int examplePrice;
    bool pressedOnExample = false;
    public GameObject panel;
    public TextMeshProUGUI panelText;
    public static ExampleManager exampleButton;
    private void Awake()
    {
        if (exampleButton == null)
            exampleButton = this;
    }
    /*
    GlobalScript GlobalScript;
  //  public GameObject Image;
    TextDisplayer TextDisplayer;
   //public GameObject Canvas;
   public Text ExampleText;*/
    // Start is called before the first frame update
    void Start()
    {
        ResetExampleOnNewScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetExampleOnNewScreen()
    {
        pressedOnExample = false;
        OnXPress();
        if(GlobalScript.Instance.chosenEntery.example == string.Empty)
        {
            gameObject.SetActive(false);
            return;
        }
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<Text>().text = "UNLOCK EXAMPLE";
        TokenSystem.ResetPanels();
    } 
    public void OnExamplePress()
    {
        if (!GameInspector.currentGameInspector.IsConnectedToInternet())
            return;
        if (!pressedOnExample)
        {
            if (!TokenSystem.AddTokensToCoin(-examplePrice))
                return;
        }
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
        PressedButton.SetActive(false);

        pressedOnExample = true;

        panel.SetActive(true);
        FillExamplePanel();

    }

    void FillExamplePanel()
    {
        panelText.text = GlobalScript.Instance.chosenEntery.example;
        TextDisplayer.CleanTMPro(panelText, GlobalScript.Instance.chosenEntery.word);
    }
    public void OnXPress()
    {
        panel.SetActive(false);
        resetExampleButtonAfterPanel();
    }
    public void resetExampleButtonAfterPanel()
    {
        gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetComponent<Text>().text = "WATCH EXAMPLE";
    }
}
