using System.Collections;
using System.Collections.Generic;
using AmazonAds;
using UnityEngine;

public class BannerAdsManager : MonoBehaviour {
    public void InitializeBannerAds(bool useAmazon) {
        // test.Instance.Log("max init banner");
        if (useAmazon) {
            int width;
            int height;
            string slotId;
            if (MaxSdkUtils.IsTablet()) {
                width = 728;
                height = 90;
                slotId = ApplovinSDKManager.Instance().bannerAdUnitId_Amazon;
            }
            else {
                width = 320;
                height = 50;
                slotId = ApplovinSDKManager.Instance().bannerAdUnitId_Amazon;
            }

            var apsBanner = new APSBannerAdRequest(width, height, slotId);
            apsBanner.onSuccess += (adResponse) => {
                MaxSdk.SetBannerLocalExtraParameter(ApplovinSDKManager.Instance().bannerAdUnitId, "amazon_ad_response", adResponse.GetResponse());
                CreateMaxBannerAd();
            };
            apsBanner.onFailedWithError += (adError) => {
                MaxSdk.SetBannerLocalExtraParameter(ApplovinSDKManager.Instance().bannerAdUnitId, "amazon_ad_error", adError.GetAdError());
                CreateMaxBannerAd();
            };

            apsBanner.LoadAd();
        }
        else {
            CreateMaxBannerAd();
        }
    }

    public void CreateMaxBannerAd() {
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(ApplovinSDKManager.Instance().bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(ApplovinSDKManager.Instance().bannerAdUnitId, new Color(0, 0, 0, 0f));

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        MaxSdk.SetBannerWidth(ApplovinSDKManager.Instance().bannerAdUnitId, 330);
        MaxSdk.SetBannerPlacement(ApplovinSDKManager.Instance().bannerAdUnitId, "MY_PLACEMENT");
        ShowBannerAd();
    }


    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        AdjustManager.Instance().SendAdjustRevenue(adInfo);
        FireBaseManager.Instance().SendFirebaseMaxSDKAdRevenuePaidEvent(adUnitId, adInfo);
    }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    public void ShowBannerAd() {
        MaxSdk.ShowBanner(ApplovinSDKManager.Instance().bannerAdUnitId);
        MaxSdk.StartBannerAutoRefresh(ApplovinSDKManager.Instance().bannerAdUnitId);
    }

    public void HideBannerAd() {
        MaxSdk.HideBanner(ApplovinSDKManager.Instance().bannerAdUnitId);
    }
}
