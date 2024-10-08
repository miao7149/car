
using System;
using AmazonAds;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AmazonMaxDemo : MonoBehaviour {

    private const string maxKey = "l-_TbRRFRIhI2bN388lTNzh0k_83nqhSLMkFs2ATgT_y4GPxCqSQOdiDV3WgHf01C4N9r53JvUp-N_65kdcdro";
#if UNITY_ANDROID
    private const string appId = "7873ab072f0647b8837748312c7b8b5a";

    private const string maxBannerAdId = "989798cb31a0d25f";
    private const string maxInterstitialAdId = "7e3a01318c888038";
    private const string maxVideoAdId = "09d9041492d1d0d9";

    private const string amazonBannerSlotId = "ed3b9f16-4497-4001-be7d-2e8ca679ee73"; //320x50
    private const string amzonInterstitialSlotId = "394133e6-27fe-477d-816b-4a00cdaa54b6";
    private const string amazonInterstitialVideoSlotId = "b9f9a2aa-72d8-4cb3-83db-949ebb53836f";
    private const string amazonRewardedVideoSlotId = "1ed9fa0b-3cf0-4326-8c35-c0e9ddcdb765";
#else
    private const string appId = "c5f20fe6e37146b08749d09bb2b6a4dd";

	private const string maxBannerAdId = "d7dc4c6c1d6886fb";
	private const string maxInterstitialAdId = "928de5b2fa152dac";
    private const string maxVideoAdId = "57e0224b0c29607c";

	private const string amazonBannerSlotId = "88e6293b-0bf0-43fc-947b-925babe7bf3f"; //320x50
	private const string amzonInterstitialSlotId = "424c37b6-38e0-4076-94e6-0933a6213496";
    private const string amazonInterstitialVideoSlotId = "671086df-06f2-4ee7-86f6-e578d10b3128";
    private const string amazonRewardedVideoSlotId = "08892e57-35ff-450c-8b35-4d261251f7c7";
#endif

    public UnityEngine.UI.Button isInitializedBut;
    private bool isAutoRefresh = true;
    private bool isFirstInterstitialRequest = true;
    private bool isFirstVideoInterstitialRequest = true;
    private bool isFirstRewardedVideoRequest = true;

    private APSBannerAdRequest bannerAdRequest;
    private APSInterstitialAdRequest interstitialAdRequest;
    private APSVideoAdRequest interstitialVideoAdRequest;
    private APSVideoAdRequest rewardedVideoAdRequest;

    public void InitializeMax () {
        Amazon.Initialize(appId);
        Amazon.EnableTesting(true);
        Amazon.EnableLogging(true);
        Amazon.UseGeoLocation(true);
        Amazon.IsLocationEnabled();
        Amazon.SetMRAIDPolicy(Amazon.MRAIDPolicy.CUSTOM);
        Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
        Amazon.SetMRAIDSupportedVersions(new string[] { "1.0", "2.0", "3.0" }); 

        MaxSdk.SetSdkKey(maxKey);
        MaxSdk.InitializeSdk();
        MaxSdk.SetCreativeDebuggerEnabled(true);
        MaxSdk.SetVerboseLogging(true);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
    }

    public void IsInitialized(){
        if (isInitializedBut == null ) return;
        if( Amazon.IsInitialized() ) {
            isInitializedBut.GetComponent<UnityEngine.UI.Image>().color = Color.green;
        } else {
            isInitializedBut.GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
    }

    public void RequestInterstitial () {
        if (isFirstInterstitialRequest) {
            isFirstInterstitialRequest = false;
            interstitialAdRequest = new APSInterstitialAdRequest(amzonInterstitialSlotId);

            interstitialAdRequest.onSuccess += (adResponse) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(maxInterstitialAdId, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.LoadInterstitial(maxInterstitialAdId);
            };
            interstitialAdRequest.onFailedWithError += (adError) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(maxInterstitialAdId, "amazon_ad_error", adError.GetAdError());
                MaxSdk.LoadInterstitial(maxInterstitialAdId);
            };

            interstitialAdRequest.LoadAd();
        } else {
            MaxSdk.LoadInterstitial(maxInterstitialAdId);
        }
    }

    private void CreateMaxBannerAd()
    {
        MaxSdk.CreateBanner(maxBannerAdId, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerPlacement(maxBannerAdId, "MY_BANNER_PLACEMENT");
    }

    public void RequestBanner () {
        const int width = 320;
        const int height = 50;

        bannerAdRequest = new APSBannerAdRequest(width, height, amazonBannerSlotId);
        bannerAdRequest.onFailedWithError += (adError) =>
        {
            MaxSdk.SetBannerLocalExtraParameter(maxBannerAdId, "amazon_ad_error", adError.GetAdError());
            CreateMaxBannerAd();
            bannerAdRequest.Dispose();
        };
        bannerAdRequest.onSuccess += (adResponse) =>
        {
            MaxSdk.SetBannerLocalExtraParameter(maxBannerAdId, "amazon_ad_response", adResponse.GetResponse());
            CreateMaxBannerAd();
            bannerAdRequest.Dispose();
        };
        bannerAdRequest.LoadAd();
    }

    public void RequestInterstitialVideo () {
        if(isFirstVideoInterstitialRequest) {
            isFirstVideoInterstitialRequest = false;
            interstitialVideoAdRequest = new APSVideoAdRequest(320, 480, amazonInterstitialVideoSlotId);

            interstitialVideoAdRequest.onSuccess += (adResponse) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(maxInterstitialAdId, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.LoadInterstitial(maxInterstitialAdId);
            };
            interstitialVideoAdRequest.onFailedWithError += (adError) =>
            {
                MaxSdk.SetInterstitialLocalExtraParameter(maxInterstitialAdId, "amazon_ad_error", adError.GetAdError());
                MaxSdk.LoadInterstitial(maxInterstitialAdId);
            };

            interstitialVideoAdRequest.LoadAd();
        } else {
            MaxSdk.LoadInterstitial(maxInterstitialAdId);
        }
    }

    public void RequestRewardedVideo () {
        if (isFirstRewardedVideoRequest) {
            isFirstRewardedVideoRequest = false;
            rewardedVideoAdRequest = new APSVideoAdRequest(320, 480, amazonRewardedVideoSlotId);

            rewardedVideoAdRequest.onSuccess += (adResponse) =>
            {
                MaxSdk.SetRewardedAdLocalExtraParameter(maxVideoAdId, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.LoadRewardedAd(maxVideoAdId);
            };
            rewardedVideoAdRequest.onFailedWithError += (adError) =>
            {
                MaxSdk.SetRewardedAdLocalExtraParameter(maxVideoAdId, "amazon_ad_error", adError.GetAdError());
                MaxSdk.LoadRewardedAd(maxVideoAdId);
            };

            rewardedVideoAdRequest.LoadAd();
        } else {
            MaxSdk.LoadRewardedAd(maxVideoAdId);
        }
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.ShowBanner(maxBannerAdId);
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnInterstitialLoadedEvent:" + MaxSdk.IsInterstitialReady(maxInterstitialAdId));
        if (MaxSdk.IsInterstitialReady(maxInterstitialAdId))
        {
            MaxSdk.ShowInterstitial(maxInterstitialAdId);
        }
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log("OnInterstitialLoadFailedEvent");
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnInterstitialDisplayedEvent");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnInterstitialAdFailedToDisplayEvent");
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnInterstitialClickedEvent");
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnInterstitialHiddenEvent");
    }

    private void ShowRewardedAd()
    {
        Debug.Log("ShowRewardedAd:" + MaxSdk.IsRewardedAdReady(maxVideoAdId));
        if (MaxSdk.IsRewardedAdReady(maxVideoAdId))
        {
            MaxSdk.ShowRewardedAd(maxVideoAdId);
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnRewardedAdLoadedEvent");
        ShowRewardedAd();
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log("OnRewardedAdFailedEvent");

    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        //MaxSdk.LoadRewardedAd(maxVideoAdId);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        //MaxSdk.LoadRewardedAd(RewardedAdUnitId);
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("HERE:Rewarded ad received reward");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("OnRewardedAdRevenuePaidEvent");
    }

    public void goBack() 
    {
        MaxSdk.DestroyBanner(maxBannerAdId);
        SceneManager.LoadScene(0);
    }
}