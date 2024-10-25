using System;
using System.Collections.Generic;
using AmazonAds;
using UnityEngine;

public class InterstitialAdsManager : MonoBehaviour {
    int retryAttempt;
    bool useAmazon = false;

    public void InitializeInterstitialAds(bool useAmazon) {
        this.useAmazon = useAmazon;
        //test.Instance.Log("max init interstitial");
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private bool IsFirstLoad = true;

    private void LoadInterstitial() {
        if (useAmazon) {
            if (IsFirstLoad) {
                IsFirstLoad = false;

                var interstitialAd = new APSInterstitialAdRequest(ApplovinSDKManager.Instance().interstitialAdUnitId_Amazon);
                interstitialAd.onSuccess += (adResponse) => {
                    MaxSdk.SetInterstitialLocalExtraParameter(ApplovinSDKManager.Instance().interstitialAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadInterstitial(ApplovinSDKManager.Instance().interstitialAdUnitId);
                };
                interstitialAd.onFailedWithError += (adError) => {
                    MaxSdk.SetInterstitialLocalExtraParameter(ApplovinSDKManager.Instance().interstitialAdUnitId, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadInterstitial(ApplovinSDKManager.Instance().interstitialAdUnitId);
                };

                interstitialAd.LoadAd();
            }
            else {
                MaxSdk.LoadInterstitial(ApplovinSDKManager.Instance().interstitialAdUnitId);
            }
        }
        else {
            MaxSdk.LoadInterstitial(ApplovinSDKManager.Instance().interstitialAdUnitId);
        }
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
        // Interstitial ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
        closeCb?.Invoke();
        StartCoroutine(LogHelper.LogToServer("GameAdInsertEnd", new Dictionary<string, object>() {
            { "LevelId", GlobalManager.Instance.CurrentLevel },
            { "ModuleId", module },
            { "CoinCount", GlobalManager.Instance.PlayerCoin }
        }));
        closeCb = null;
        AudioSettings.Reset(AudioSettings.GetConfiguration());
        AudioManager.Instance.ResumeBackgroundMusic();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
        closeCb?.Invoke();
        StartCoroutine(LogHelper.LogToServer("GameAdInsertEnd", new Dictionary<string, object>() {
            { "LevelId", GlobalManager.Instance.CurrentLevel },
            { "ModuleId", module },
            { "CoinCount", GlobalManager.Instance.PlayerCoin }
        }));
        closeCb = null;
        AudioSettings.Reset(AudioSettings.GetConfiguration());
        AudioManager.Instance.ResumeBackgroundMusic();
    }

    private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        AdjustManager.Instance().SendAdjustRevenue(adInfo);
        FireBaseManager.Instance().SendFirebaseMaxSDKAdRevenuePaidEvent(adUnitId, adInfo);
    }

    private Action closeCb;
    private string module;

    public void ShowInterstitialAd(string module, Action cb) {
        this.module = module;
        closeCb = cb;
        // Show the interstitial ad if it is ready
        float t = Time.time - ApplovinSDKManager.Instance().lastShowTime;
        Debug.Log(t);
        if (t < ApplovinSDKManager.Instance().delay) {
            closeCb?.Invoke();
            closeCb = null;
            return;
        }

        if (MaxSdk.IsInterstitialReady(ApplovinSDKManager.Instance().interstitialAdUnitId)) {
            StartCoroutine(LogHelper.LogToServer("GameAdInsertStart", new Dictionary<string, object>() {
                { "LevelId", GlobalManager.Instance.CurrentLevel },
                { "ModuleId", module },
                { "CoinCount", GlobalManager.Instance.PlayerCoin }
            }));
            MaxSdk.ShowInterstitial(ApplovinSDKManager.Instance().interstitialAdUnitId);
            ApplovinSDKManager.Instance().lastShowTime = Time.time;
        }
        else {
            closeCb?.Invoke();
            closeCb = null;
        }
    }
}
