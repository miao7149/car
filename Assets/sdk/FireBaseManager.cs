using System;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class FireBaseManager {
    private static FireBaseManager instance = null;

    public static FireBaseManager Instance() {
        if (instance == null) {
            instance = new FireBaseManager();
        }

        return instance;
    }

    // private Firebase.FirebaseApp app;

    public void InitSDK() {
#if UNITY_EDITOR
        return;
#endif
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                var app = Firebase.FirebaseApp.DefaultInstance;
            }
            else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    // public void FetchData(Action<bool> cb) {
    //     FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().ContinueWith(task => {
    //         if (task.IsFaulted) {
    //             // test.Instance.Log("Remote Fetch failed.");
    //             cb?.Invoke(false);
    //         }
    //         else {
    //             //test.Instance.Log("Remote Fetch succeeded.");
    //
    //             bool useAmazon = FirebaseRemoteConfig.DefaultInstance.GetValue("useAmazon").BooleanValue;
    //             if (useAmazon) {
    //                 ApplovinSDKManager.Instance().bannerAdUnitId_Amazon = FirebaseRemoteConfig.DefaultInstance.GetValue("amazonBanner").StringValue;
    //                 ApplovinSDKManager.Instance().interstitialAdUnitId_Amazon = FirebaseRemoteConfig.DefaultInstance.GetValue("amazoninterstitial").StringValue;
    //                 ApplovinSDKManager.Instance().rewardadUnitId_Amazon = FirebaseRemoteConfig.DefaultInstance.GetValue("amazonReward").StringValue;
    //                 ApplovinSDKManager.Instance().amazonAppId = FirebaseRemoteConfig.DefaultInstance.GetValue("amazonAppID").StringValue;
    //                 cb?.Invoke(true);
    //             }
    //             else {
    //                 cb?.Invoke(false);
    //             }
    //         }
    //     });
    // }

    public void ApplyRemoteConfig() {
        string myValue = FirebaseRemoteConfig.DefaultInstance.GetValue("my_key").StringValue;
        Debug.Log("Remote Config value: " + myValue);
    }

    public void LogEvent(string name) {
        FirebaseAnalytics.LogEvent(name);
    }

    public void SendFirebaseMaxSDKAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData) {
        double currentImpressionRevenue = impressionData.Revenue; // In USD
        Firebase.Analytics.Parameter[] Parameters = {
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdPlatform, "applovin"),
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdSource, impressionData.NetworkName),
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdFormat, impressionData.AdFormat),
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdUnitName, adUnitId),
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, impressionData.Revenue),
            new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency, "USD") // All Applovin revenue is sent in USD
        };
        //if (!impressionData.NetworkName.Equals("Google AdMob"))
        //{
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, Parameters); // 给ARO用
        //}
        FirebaseAnalytics.LogEvent("Ad_Impression_Revenue", Parameters); // 给Taichi用


        float previousTaichiTroasCache = PlayerPrefs.GetFloat("TaichiTroasCache", 0); //App本地存储用于累计tROAS的缓存值
        float currentTaichiTroasCache = (float)(previousTaichiTroasCache + currentImpressionRevenue); //累加tROAS的缓存值
        //check是否应该发送TaichitROAS事件
        if (currentTaichiTroasCache >= 0.01) //如果超过0.01就触发一次tROAS taichi事件
        {
            Firebase.Analytics.Parameter[] roasParameters = {
                new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, currentTaichiTroasCache), //(Required)tROAS事件必须带Double类型的Value
                new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),
            };
            FirebaseAnalytics.LogEvent("Total_Ads_Revenue_001", roasParameters); // 给Taichi用
            PlayerPrefs.DeleteKey("TaichiTroasCache"); //重新清零，开始计算
        }
        else {
            PlayerPrefs.SetFloat("TaichiTroasCache", currentTaichiTroasCache); //先存着直到超过0.01才发送
        }

        //debug_Text.text += "   <" + "MaxSDK: OnInterstitialAdRevenuePaidEvent: " + "Revenue = " + impressionData.Revenue + "   TaichiTroasCache = " + currentTaichiTroasCache + ">";
    }
}
