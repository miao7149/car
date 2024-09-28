using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSdkManager : MonoBehaviour {
    // string Interstitial = "d3d83b746e90e0dd";
    // string Reward = "f2ab973bd568a3b9";
    // string Banner = "62fd449a0f51568f";

    // Start is called before the first frame update
    public static CommonSdkManager Instance;

    private bool useAmazon = false;


    void Start() {
#if UNITY_EDITOR
        useAmazon = false;
#elif UNITY_ANDROID
        useAmazon = false;
#elif UNITY_IOS
        useAmazon = true;
#endif

        DontDestroyOnLoad(gameObject);
        Instance = this;
        FireBaseManager.Instance().InitSDK();
        ApplovinSDKManager.Instance().Init(useAmazon, () => {
        });
    }
}
