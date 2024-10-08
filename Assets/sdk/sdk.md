# firebase
#### 下载配置文件放入Asset文件夹
    对于 iOS - GoogleService-Info.plist。
    对于 Android - google-services.json。
#### 下载sdk 
    https://firebase.google.com/docs/unity/setup?hl=zh-cn
#### 接入
    FirebaseAnalytics.unitypackage
    FirebaseRemoteConfig.unitypackage

#### 添加初始化代码
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
    var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.
            app = Firebase.FirebaseApp.DefaultInstance;
    
            // Set a flag here to indicate whether Firebase is ready to use by your app.
        } else {
            UnityEngine.Debug.LogError(System.String.Format(
            "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
    });

# applovin
#### 下载sdk
    https://developers.applovin.com/en/unity/overview/integration/
#### 接入
    AppLovin-MAX-Unity-Plugin-6.6.3-Android-12.6.1-iOS-12.6.1.unitypackage
#### 设置
    对于 Android 版本，AppLovin MAX 插件要求您启用Jetifier。要启用 Jetifier，请执行以下步骤：
    在 Unity 中，选择Assets > External Dependency Manager > Android Resolver > Settings。
    在出现的Android Resolver Settings对话框中，选中Use Jetifier。
    单击“确定”。
#### 设置applovin 添加sdk key
    hMvQ0_RopyUtvvfyZnlJxztPo2q_KJgtQf9Lbbq3go1sLcrlDDcSzPT5ZTBpI3RJaSAeq7LkiHPAX5D06lC7H5
#### 初始化
    MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
        // AppLovin SDK is initialized, start loading ads
    };

    MaxSdk.SetSdkKey("hMvQ0_RopyUtvvfyZnlJxztPo2q_KJgtQf9Lbbq3go1sLcrlDDcSzPT5ZTBpI3RJaSAeq7LkiHPAX5D06lC7H5");
    MaxSdk.InitializeSdk();
#### 隐私政策
    applovin 设置里面勾选 添加网址
#### 添加代码

# adjust
#### 下载sdk
    https://github.com/adjust/unity_sdk/releases/tag/v5.0.1
#### 接入
    AdjustSdk.unitypackage
    把 adjust.prefab 拖入初始场景
#### android 设置
    adjust editor Dependencies.xml 文件中添加以下代码：
    <androidPackage spec="com.google.android.gms:play-services-ads-identifier:18.0.1" />
    <androidPackage spec="com.google.android.gms:play-services-appset:16.0.2" />
##### 混淆
    -keep public class com.adjust.sdk.**{ *; }
    -keep class com.google.android.gms.common.ConnectionResult {
    int SUCCESS;
    }
    -keep class com.google.android.gms.ads.identifier.AdvertisingIdClient {
    com.google.android.gms.ads.identifier.AdvertisingIdClient$Info getAdvertisingIdInfo(android.content.Context);
    }
    -keep class com.google.android.gms.ads.identifier.AdvertisingIdClient$Info {
    java.lang.String getId();
    boolean isLimitAdTrackingEnabled();
    }
    -keep public class com.android.installreferrer.**{ *; }
##### 权限
    android.permission.INTERNET
    android.permission.ACCESS_NETWORK_STATE
    com.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE 
    com.google.android.gms.permission.AD_ID

### amazon
#### 下载sdk
    https://developers.applovin.com/en/max/unity/amazon-publisher-services-integration-instructions
#### 接入
    APS_Core_1_9_1.unitypackage
    APSAppLovinMediation_1_9_1.unitypackage
#### 初始化
    Amazon.Initialize(amazonAppId);
    Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.MAX));
#### 接入代码

### pubmatic
#### 下载sdk 导入
    AppLovinPubMaticAdapterPlugin-v1.1.0.unitypackage
    OpenWrapSDKUnityPlugin-v3.1.0.unitypackage
#### 添加商店地址 代码
#### 添加权限permission

# facebook
#### 下载sdk
    https://developers.facebook.com/docs/unity/
#### 接入
    facebook-unity-sdk-17.0.1.unitypackage
#### 设置
    工具栏 facebookSetting 添加appid  Client token


# 注意事项

    android 最小版本24 不然报错
    playerSetting 打开Custom Main Manifest , Custom Main Gradle Template , Custom Gradle Properties Template,Custom Gradle Setting Template ,Custom proguard file
    ios player setting 里面的 target minimum version 改成13

    遇到提交报错failed to cloud sign libswift_concurrency.dylib
    go to general tab and in Target > UnityFramework > Frameworks and Libraries remove the AVFoundation.framework and add it again


    ios build 后先 pod install --repo-update 启动xcworkspace
#### podfile中 
    pod 'AppLovinMediationGoogleAdapter', '11.3.0.0'     -old 11.8.0.0
    pod 'AppLovinMediationIronSourceAdapter', '8.0.0.0.0'  -old 8.2.0.0.0
    pod 'AppLovinMediationFacebookAdapter', '6.15.0.0'   -old 6.15.2.0

    pod 成功后把framework全部引用到 项目里面(一般缺 adjustsig.fw)
