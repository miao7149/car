#if UNITY_ANDROID
/*
* PubMatic Inc. ("PubMatic") CONFIDENTIAL
* Unpublished Copyright (c) 2006-2022 PubMatic, All Rights Reserved.
*
* NOTICE:  All information contained herein is, and remains the property of PubMatic. The intellectual and technical concepts contained
* herein are proprietary to PubMatic and may be covered by U.S. and Foreign Patents, patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material is strictly forbidden unless prior written permission is obtained
* from PubMatic.  Access to the source code contained herein is hereby forbidden to anyone except current PubMatic employees, managers or contractors who have executed
* Confidentiality and Non-disclosure agreements explicitly covering such access or to such other persons whom are directly authorized by PubMatic to access the source code and are subject to confidentiality and nondisclosure obligations with respect to the source code.
*
* The copyright notice above does not evidence any actual or intended publication or disclosure  of  this source code, which includes
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION, PUBLIC  PERFORMANCE,
* OR PUBLIC DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

namespace OpenWrapSDK.Android
{
    /// <summary>
    /// Contains android specific constants
    /// </summary>
    internal class POBConstants
    {
        internal readonly static string OpenWrapSDKClassName = "com.pubmatic.sdk.common.OpenWrapSDK";
        internal readonly static string POBLogLevelClassName = "com.pubmatic.sdk.common.OpenWrapSDK$LogLevel";
        internal readonly static string POBApplicationInfoClassName = "com.pubmatic.sdk.common.models.POBApplicationInfo";
        internal readonly static string POBUserInfoClassName = "com.pubmatic.sdk.common.models.POBUserInfo";
        internal readonly static string POBRequestClassName = "com.pubmatic.sdk.openwrap.core.POBRequest";
        internal readonly static string POBImpressionClassName = "com.pubmatic.sdk.openwrap.core.POBImpression";
        internal readonly static string POBInterstitialClassName = "com.pubmatic.unity.openwrapsdk.POBUnityInterstitial";
        internal readonly static string POBInterstitialAdCallbackInterfaceName = "com.pubmatic.unity.openwrapsdk.POBUnityInterstitialListener";
        internal readonly static string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
        internal readonly static string POBGenderClassName = "com.pubmatic.sdk.common.models.POBUserInfo$Gender";
        internal readonly static string POBLocationClassName = "com.pubmatic.sdk.common.models.POBLocation";
        internal readonly static string POBLocationSourceClassName = "com.pubmatic.sdk.common.models.POBLocation$Source";
        internal readonly static string POBBannerViewClassName = "com.pubmatic.unity.openwrapsdk.POBUnityBannerView";
        internal readonly static string POBBannerAdCallbackInterfaceName = "com.pubmatic.unity.openwrapsdk.POBUnityBannerListener";
        internal readonly static string POBBannerAdSizeClassName = "com.pubmatic.sdk.common.POBAdSize";
        internal readonly static string POBSegmentClassName = "com.pubmatic.sdk.common.models.POBSegment";
        internal readonly static string POBRewardedAdClassName = "com.pubmatic.unity.openwrapsdk.POBUnityRewardedAd";
        internal readonly static string POBRewardedAdCallbackInterfaceName = "com.pubmatic.unity.openwrapsdk.POBUnityRewardedAdListener";
        internal readonly static string POBExternalUserIdClassName = "com.pubmatic.sdk.common.models.POBExternalUserId";
        internal readonly static string POBDataProviderClassName = "com.pubmatic.sdk.common.models.POBDataProvider";
        internal readonly static string IntegerClassName = "java.lang.Integer";

    }
}
#endif
