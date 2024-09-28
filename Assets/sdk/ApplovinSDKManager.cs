using System;
using AmazonAds;
using OpenWrapSDK;

public class ApplovinSDKManager
{
    private static ApplovinSDKManager instance = null;

    public static ApplovinSDKManager Instance()
    {
        if (instance == null) instance = new ApplovinSDKManager();
        return instance;
    }

    public InterstitialAdsManager interstitialAdsManager;
    public RewardedAdsManager rewardAdsManager;
    public BannerAdsManager bannerAdsManager;

    public float delay = 60;
    public float lastShowTime = 0;


    //账号

    private string applovinAppID = "hMvQ0_RopyUtvvfyZnlJxztPo2q_KJgtQf9Lbbq3go1sLcrlDDcSzPT5ZTBpI3RJaSAeq7LkiHPAX5D06lC7H5";

#if UNITY_IOS
    public string amazonAppId = "8eeea3b0-ac72-47f3-84ee-4c756bc93894";
    public string bannerAdUnitId = "d4fe2a67ef604cb7"; // Retrieve the ID from your account
    public string interstitialAdUnitId = "11a02df7ea7198d7";
    public string rewardadUnitId = "3ec3dedc95332434"; // Retrieve the ID from your account

    public string bannerAdUnitId_Amazon = "6d877c07-b6d3-4102-bb73-6dccad225f2d"; // Retrieve the ID from your account
    public string interstitialAdUnitId_Amazon = "45cb81f6-c683-439f-95ec-c287bbdd56ab";
    public string rewardadUnitId_Amazon = "26f9a1c3-29d9-4205-a93d-860255de339a";
#else // UNITY_ANDROID
    public string amazonAppId = "8eeea3b0-ac72-47f3-84ee-4c756bc93894";
    public string bannerAdUnitId = "863e5d8910141a9d"; // Retrieve the ID from your account
    public string interstitialAdUnitId = "ee6741b64aa1b678";
    public string rewardadUnitId = "15ecf3c291b36739";

    public string bannerAdUnitId_Amazon = ""; // Retrieve the ID from your account
    public string interstitialAdUnitId_Amazon = "";
    public string rewardadUnitId_Amazon = "";
#endif

    //public bool useAmazon = false;


    public void Init(bool useAmazon, Action cb)
    {
        //test.Instance.Log("max init");
        //amazon


        if (useAmazon)
        {
            Amazon.Initialize(amazonAppId);
            Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
        }
        //test.Instance.Log("max init 1");


        //pubmatic

        POBApplicationInfo appInfo = new POBApplicationInfo();
#if UNITY_EDITOR

#elif UNITY_IOS
        appInfo.StoreURL = new Uri("https://apps.apple.com/app/screw-jam-master-nuts-puzzle/id6677018495");
        POBOpenWrapSDK.SetApplicationInfo(appInfo);
#elif UNITY_ANDROID
        appInfo.StoreURL = new Uri("https://play.google.com/store/apps/details?id=com.game.sjm_android");
        POBOpenWrapSDK.SetApplicationInfo(appInfo);
#endif

        //test.Instance.Log("max init 2");

        bannerAdsManager = new BannerAdsManager();
        interstitialAdsManager = new InterstitialAdsManager();
        rewardAdsManager = new RewardedAdsManager();

        //test.Instance.Log("max init 3");
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            //test.Instance.Log("max init finish");

            // AppLovin SDK is initialized, start loading ads
            interstitialAdsManager.InitializeInterstitialAds(useAmazon);
            rewardAdsManager.InitializeRewardedAds(useAmazon);
            bannerAdsManager.InitializeBannerAds(useAmazon);
            cb.Invoke();
        };

        MaxSdk.SetSdkKey(applovinAppID);
        // test.Instance.Log("max init 4");
        MaxSdk.InitializeSdk();
    }
}
