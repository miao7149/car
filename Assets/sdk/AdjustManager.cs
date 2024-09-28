using AdjustSdk;

public class AdjustManager {
    private static AdjustManager instance = null;

    public static AdjustManager Instance() {
        if (instance == null) instance = new AdjustManager();
        return instance;
    }

    public void SendAdjustRevenue(MaxSdkBase.AdInfo adInfo) {
        var adRevenue = new AdjustAdRevenue("applovin_max_sdk");
        adRevenue.SetRevenue(adInfo.Revenue, "USD");
        adRevenue.AdRevenueNetwork = adInfo.NetworkName;
        adRevenue.AdRevenueUnit = adInfo.AdUnitIdentifier;
        adRevenue.AdRevenuePlacement = adInfo.Placement;

        Adjust.TrackAdRevenue(adRevenue);
    }

    // public void SendAdjustEvent(string eventName, string eventValue) {
    //     AdjustEvent adjustEvent = new AdjustEvent(eventName);
    //     adjustEvent.AddCallbackParameter("value", eventValue);
    //     Adjust.TrackEvent(adjustEvent);
    // }
    public void SendAdjustEvent(string eventName) {
        AdjustEvent adjustEvent = new AdjustEvent(eventName);
        Adjust.TrackEvent(adjustEvent);
    }
}
