using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StrikesScript : MonoBehaviour
{
     GameManager GameManager;
   // Text StrikeCounter;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager == null)
            GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.GetComponent<TextMeshProUGUI>().text = GameManager.Strikes[GameManager.index]  + "/5";
      //  StrikeCounter = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
