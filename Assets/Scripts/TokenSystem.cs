using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Threading.Tasks;

public class TokenSystem : MonoBehaviour
{
    public static float yoffset = 20f;
    public static TokenSystem currentTokenSystem;
    public GameObject animationPrefab;
    public static GameObject currentCoin;
    public static GameObject definitionCoin;
    public static GameObject menuCoin;
    private void Awake()
    {
        currentTokenSystem = this;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0:
                if(menuCoin == null)
                    menuCoin = gameObject;
                currentCoin = menuCoin;
                break;
            case 1:
                if(definitionCoin == null)
                    definitionCoin = gameObject;
                currentCoin = definitionCoin;
                break;
        }
        currentCoin.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Tokens.ToString();
    }
    public static void UpdateValueOfCoin()
    {
        currentCoin.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Tokens.ToString();
    }
    public static bool AddTokensToCoin(int numOfTokens)
    {
        if(GameManager.Tokens+numOfTokens < 0)
        {
            InsufficentFundsAnimation();
            return false;
        }
        ActivateAnimation(numOfTokens,currentCoin.transform.GetChild(3).gameObject, new Vector2(0,0));
        GameManager.Tokens += numOfTokens;
        Data.PlayerData.Tokens = GameManager.Tokens;
        currentCoin.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.Tokens.ToString();
        return true;
    }
    public static void ActivateAnimation(int num, GameObject parent,Vector2 offset)
    {
        GameObject animationCopy = Instantiate(currentTokenSystem.animationPrefab, parent.transform, true);
        animationCopy.transform.position = new Vector2(parent.transform.position.x-offset.x, parent.transform.position.y - offset.y);
        string str = string.Empty;
        if (num > 0)
            str += "+";
        str += num.ToString();
        animationCopy.GetComponent<TextMeshProUGUI>().text = str;
        animationCopy.SetActive(true);
    }
    public static void OnButtonPress()
    {
        OpenShop();

    }
    public GameObject Shop;
    public static void OpenShop()
    {
        currentTokenSystem.Shop.SetActive(true);
        MenuScript.Instance.inPanel = true;
    }
    #region ShopButtons

    public static void OnXPress()
    {
        GameObject PressedButton = EventSystem.current.currentSelectedGameObject;
        PressedButton.transform.parent.gameObject.SetActive(false);
        MenuScript.Instance.inPanel = false;
    }

    #endregion


    #region InsufficentFunds
    public GameObject insufficentFunds;
    public static void InsufficentFundsAnimation()
    {
        currentTokenSystem.insufficentFunds.SetActive(true);
        currentTokenSystem.Shop.SetActive(false);
    }
  


    #region InsufficentPanelButtons
    
    public static void OnButtonPressIn()
    {
        currentTokenSystem.insufficentFunds.SetActive(false);
        OpenShop();

    }

    #endregion
    #endregion

    public static void ResetPanels()
    {
        currentTokenSystem.insufficentFunds.SetActive(false);
        currentTokenSystem.Shop.SetActive(false);
    }
}
