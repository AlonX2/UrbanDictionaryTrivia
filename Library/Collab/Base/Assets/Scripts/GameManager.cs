using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI StrikeText;
    public int[] Strikes = new int[50];
    public bool[] BoughtButtons = new bool[50];
    public bool MegaGay;//premium = MegaGay
    public static int Tokens;
    public DateTime[] LockTime = new DateTime[50];
    public static GameManager Instance;
    GlobalScript GlobalScript;
    //  System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
    //   bool FilterSet = false;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            MobileAds.Initialize(InitStatus => { });
            Entery.persistentDataPath = Application.persistentDataPath;
            ActivateBanner();
            Debug.Log(Tokens);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < Strikes.Length; i++)
        {
            Strikes[i] = 0;
        }
        for (int i = 0; i < BoughtButtons.Length; i++)
        {
            BoughtButtons[i] = false;
        }
        //2020/09/01 17:37:45.966 27986 27986 Info Ads Use RequestConfiguration.Builder().setTestDeviceIds(Arrays.asList("BCD4C11A23F869602FBA3B8925EB45C6") to get test ads on this device.

        List<string> deviceIds = new List<string>();
        deviceIds.Add("BCD4C11A23F869602FBA3B8925EB45C6");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);




    }
    #region dontTouch
    public void MatchData()
    {
        Strikes = Data.PlayerData.Strikes;
        MegaGay = Data.PlayerData.MegaGay;
        Tokens = Data.PlayerData.Tokens;
        LockTime = Data.PlayerData.LockedTime;
        BoughtButtons = Data.PlayerData.BoughtButtons;
        MenuScript.Instance.CheckStrikes();

    }
    public void FirstWrite()
    {
        Data.PlayerData.Strikes = Strikes;
        Data.PlayerData.MegaGay = MegaGay;
        Data.PlayerData.Tokens = Tokens;
        Data.PlayerData.LockedTime = LockTime;
        Data.PlayerData.BoughtButtons = BoughtButtons;
        MenuScript.Instance.CheckStrikes();

    }
    // Start is called before the first frame update
    void Start()
    {

        GlobalScript = GameObject.Find("GlobalManager").GetComponent<GlobalScript>();
        CreateAndLoadRewardedAd();
    }

    // Update is called once per frame
    void Update()
    {



    }
    public void SceneChange(string TargetScene)
    {
        SceneManager.LoadScene(TargetScene);
        //  if (SceneManager.GetActiveScene().name == "DefinitionScreen" && StrikeText == null)


    }
    public GameObject FindObject(string name)
    {
        return GameObject.Find(name);
    }
    public void SetFilter(string filter, int index)
    {
        filter += "Filter";
        GlobalScript.Instance.filter = filter;
        GlobalScript.Instance.LoadQ(GameInspector.GetFromSavedInCategory(filter), filter);
        this.index = index;

    }
    public void StrikeCall()
    {
        if (StrikeText == null)
            StrikeText = GameObject.Find("MainCanvas").transform.Find("TopCanvas").Find("Strikes").Find("counter").gameObject.GetComponent<TextMeshProUGUI>();
        Strikes[index]++;
        Data.PlayerData.Strikes = Strikes;
        StrikeText.text = Strikes[index] + "/5";
        if (Strikes[index] == 5)
        {
            GameInspector.AddToSavedQuestions(new Global(GlobalScript.Instance));
            SceneChange("MainMenu");
            LockTime[index] = DateTime.Now;
            Data.PlayerData.LockedTime = LockTime;
        }
        TokenSystem.ActivateAnimation(+1, StrikeText.transform.parent.GetChild(2).gameObject,new Vector2(0,0));
    }
    public void StrikeMinus()
    {
        if (Strikes[index] == 0)
            return;
        if (StrikeText == null)
            StrikeText = GameObject.Find("MainCanvas").transform.Find("TopCanvas").Find("Strikes").Find("counter").gameObject.GetComponent<TextMeshProUGUI>();
        Strikes[index]--;
        StrikeText.text = Strikes[index] + "/5";
        TokenSystem.ActivateAnimation(-1, StrikeText.gameObject, new Vector2(5, 20));

    }
    #endregion

    #region Ad Manager
    public void ActivateBanner()
    {
        RequestBanner();
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
         
        // Load the banner with the request.
        this.bannerView.LoadAd(request);
        Debug.Log("Loaded banner");


    }
    const string androidBannerId = "ca-app-pub-5468583396030938/5876409029";
    const string appleBannerId = "ca-app-pub-3940256099942544/2934735716";
    private BannerView bannerView;
    public void RequestBanner()
    {
        string adUnityId;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                adUnityId = androidBannerId;
                break;
            case RuntimePlatform.IPhonePlayer:
                adUnityId = appleBannerId;
                break;
            default:
                adUnityId = "Unknown";
                break;
        }
        this.bannerView = new BannerView(adUnityId, AdSize.Banner, AdPosition.Bottom);

    }
    private const string rewardAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    public RewardedAd rewardedAd;
    public void CreateAndLoadRewardedAd()
    {
        
        rewardedAd = new RewardedAd(rewardAdUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }
    bool Finished = false;
    public async Task<bool> ShowRewardedAd()
    {
        watched = false;

        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            Debug.Log("Loaded and done");
            while (!Finished)
            {
              await Task.Delay(25);
            }
        }
        else
        {
            Debug.Log("not loaded in time");
        }
        Finished = false;
        
        return watched;
        
    }
    
    public static bool watched;
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        watched = true; 
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
        Finished= true;
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
        left = numberOfAdTries;
    }
    public int numberOfAdTries;
    int left=-1;
    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        if (left == -1)
            left = numberOfAdTries;
        left--;
        if (left > 0)
            CreateAndLoadRewardedAd();
    }

    #endregion
}
