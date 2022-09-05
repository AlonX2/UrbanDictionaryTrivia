using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplayer : MonoBehaviour
{
    GlobalScript GlobalScript;
    Text text;
    // Start is called before the first frame update

        
    public static void DisplayToCanvas(RectTransform c)
    {
        GlobalScript globalScript = GlobalScript.Instance;
        Text definition = c.transform.GetChild(0).GetComponent<Text>();
        definition.text = globalScript.chosenEntery.definition;
        CleanText(definition, globalScript.chosenEntery.lowercase_word);
        ButtonManager.ButtonDisplayer(c.transform.GetChild(1).gameObject);
        if (IsOverflowing(definition))
        {
            definition.resizeTextForBestFit = true;
        }
    }
    public static bool IsOverflowing(Text text)
    {
        GUIStyle height_calculator = new GUIStyle();
        GUIContent guiTextContent = new GUIContent(text.GetComponent<Text>().text);

        float textWidth = text.GetComponent<Text>().preferredWidth;
        float textHeight = height_calculator.CalcHeight(guiTextContent, textWidth);
        if(text.GetComponent<RectTransform>().rect.width < textWidth || text.GetComponent<RectTransform>().rect.width < textHeight)
        {
            return true;
        }
        return false;
    } 
    public static void CleanText(Text Textcomp, string word)
    {
        Textcomp.text = Textcomp.text.ToUpper();
        word = word.ToUpper();
        while (Textcomp.text.Contains(word))
        {
            Textcomp.text = Textcomp.text.Replace(word, "<color=green>/////</color>");//deletes [] and makes the word into ////.
        }
        while (Textcomp.text.Contains("["))
        {
            Textcomp.text = Textcomp.text.Replace("[", string.Empty);
        }
        while (Textcomp.text.Contains("]"))
        {
            Textcomp.text = Textcomp.text.Replace("]", string.Empty);
        }
    }
    public static void CleanTMPro(TextMeshProUGUI Textcomp, string word)
    {
        Textcomp.text = Textcomp.text.ToUpper();
        word = word.ToUpper();
        while (Textcomp.text.Contains(word))
        {
            Textcomp.text = Textcomp.text.Replace(word, "<color=green>/////</color>");//deletes [] and makes the word into ////.
        }
        while (Textcomp.text.Contains("["))
        {
            Textcomp.text = Textcomp.text.Replace("[", string.Empty);
        }
        while (Textcomp.text.Contains("]"))
        {
            Textcomp.text = Textcomp.text.Replace("]", string.Empty);
        }
    }
    //Return whether the text is too wide for the current field.  

    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 80), "Test"))
        {
            PrintPos();
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        
    }
}
