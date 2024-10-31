using System;
using System.Collections;
using System.Collections.Generic;
using AmazonAds;
using UnityEngine;

public class RewardedAdsManager : MonoBehaviour {
    int retryAttempt;

    bool useAmazon = false;

    public void InitializeRewardedAds(bool useAmazon) {
        this.useAmazon = useAmazon;
        //  test.Instance.Log("max init reward");
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private bool IsFirstLoad = true;

    private void LoadRewardedAd() {
        if (useAmazon) {
            if (IsFirstLoad) {
                IsFirstLoad = false;

                var rewardedVideoAd = new APSVideoAdRequest(320, 480, ApplovinSDKManager.Instance().rewardadUnitId_Amazon);
                rewardedVideoAd.onSuccess += (adResponse) => {
                    MaxSdk.SetRewardedAdLocalExtraParameter(ApplovinSDKManager.Instance().rewardadUnitId, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadRewardedAd(ApplovinSDKManager.Instance().rewardadUnitId);
                };
                rewardedVideoAd.onFailedWithError += (adError) => {
                    MaxSdk.SetRewardedAdLocalExtraParameter(ApplovinSDKManager.Instance().rewardadUnitId, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadRewardedAd(ApplovinSDKManager.Instance().rewardadUnitId);
                };

                rewardedVideoAd.LoadAd();
            }
            else {
                MaxSdk.LoadRewardedAd(ApplovinSDKManager.Instance().rewardadUnitId);
            }
        }
        else {
            MaxSdk.LoadRewardedAd(ApplovinSDKManager.Instance().rewardadUnitId);
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
        // Rewarded ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
        failedCallBack.Invoke();
        failedCallBack = null;
        ApplovinSDKManager.Instance().lastShowTime = Time.time;
        AudioSettings.Reset(AudioSettings.GetConfiguration());
        AudioManager.Instance.ResumeBackgroundMusic();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        Debug.Log("广告回调：OnRewardedAdHiddenEvent");
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        if (success) {
            Debug.Log("视频广告广告完成回调");
            successCallBack.Invoke();

            CommonSdkManager.Instance.StartCoroutine(LogHelper.LogToServer("GameAdRewardEnd", new Dictionary<string, object>() {
                { "LevelId", GlobalManager.Instance.CurrentLevel },
                { "ModuleId", module },
                { "CoinCount", GlobalManager.Instance.PlayerCoin }
            }));
            Debug.Log("successCallBack ！= null");
            successCallBack = null;
            ApplovinSDKManager.Instance().lastShowTime = Time.time;
            AudioSettings.Reset(AudioSettings.GetConfiguration());
            AudioManager.Instance.ResumeBackgroundMusic();
        }
        else {
            failedCallBack.Invoke();
            failedCallBack = null;
            ApplovinSDKManager.Instance().lastShowTime = Time.time;
            AudioSettings.Reset(AudioSettings.GetConfiguration());
            AudioManager.Instance.ResumeBackgroundMusic();
        }
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) {
        // The rewarded ad displayed and the user should receive the reward.
        success = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        AdjustManager.Instance().SendAdjustRevenue(adInfo);
        FireBaseManager.Instance().SendFirebaseMaxSDKAdRevenuePaidEvent(adUnitId, adInfo);
        // Ad revenue paid. Use this callback to track user revenue.
    }

    public bool IsRewardedAdReady() {
        return MaxSdk.IsRewardedAdReady(ApplovinSDKManager.Instance().rewardadUnitId);
    }

    private Action successCallBack;
    private Action failedCallBack;
    private bool success;

    private string module;

    public void ShowRewardedAd(string module, Action success_cb, Action failed) {
        this.module = module;
        success = false;
        successCallBack = success_cb;
        failedCallBack = failed;
        if (MaxSdk.IsRewardedAdReady(ApplovinSDKManager.Instance().rewardadUnitId)) {
            CommonSdkManager.Instance.StartCoroutine(LogHelper.LogToServer("GameAdRewardStart", new Dictionary<string, object>() {
                { "LevelId", GlobalManager.Instance.CurrentLevel },
                { "ModuleId", module },
                { "CoinCount", GlobalManager.Instance.PlayerCoin }
            }));
            MaxSdk.ShowRewardedAd(ApplovinSDKManager.Instance().rewardadUnitId);
        }
        else {
            failedCallBack.Invoke();
            failedCallBack = null;
        }
    }
}
