using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelTextManager : MonoBehaviour
{
    static Text text;
    static LabelTextManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        if (text == null)
            text = gameObject.GetComponent<Text>();
    }
    public static void ChangeText()
    {
        Instance.Start();
    }
    void Start()
    {
        if (TextDisplayer.IsOverflowing(text))
            text.resizeTextForBestFit = true;
    }
    
}
