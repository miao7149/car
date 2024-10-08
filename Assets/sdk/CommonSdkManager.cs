using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSdkManager : MonoBehaviour
{
        // string Interstitial = "d3d83b746e90e0dd";
        // string Reward = "f2ab973bd568a3b9";
        // string Banner = "62fd449a0f51568f";

        // Start is called before the first frame update
        // 单例实例
        private static CommonSdkManager instance;
        public static CommonSdkManager Instance
        {
                get
                {
                        if (instance == null)
                        {
                                // 在场景中查找现有实例
                                instance = FindObjectOfType<CommonSdkManager>();

                                // 如果没有找到，创建一个新的GameObject并添加GlobalManager组件
                                if (instance == null)
                                {
                                        GameObject singleton = new GameObject("CommonSdkManager");
                                        instance = singleton.AddComponent<CommonSdkManager>();
                                        DontDestroyOnLoad(singleton);
                                }
                        }
                        return instance;
                }
        }

        private bool useAmazon = false;


        void Start()
        {
#if UNITY_EDITOR
                useAmazon = false;
#elif UNITY_ANDROID
        useAmazon = false;
#elif UNITY_IOS
        useAmazon = true;
#endif

                DontDestroyOnLoad(gameObject);
                FireBaseManager.Instance().InitSDK();
                ApplovinSDKManager.Instance().Init(useAmazon, () =>
                {
                });
        }
}
