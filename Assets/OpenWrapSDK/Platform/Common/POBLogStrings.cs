#if UNITY_IOS || UNITY_ANDROID
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
* information that is confidential and/or proprietary, and is a trade secret, of  PubMatic.   ANY REPRODUCTION, MODIFICATION, DISTRIBUTION,  PERFORMANCE,
* OR DISPLAY OF OR THROUGH USE  OF THIS  SOURCE CODE  WITHOUT  THE EXPRESS WRITTEN CONSENT OF PUBMATIC IS STRICTLY PROHIBITED, AND IN VIOLATION OF APPLICABLE
* LAWS AND INTERNATIONAL TREATIES.  THE RECEIPT OR POSSESSION OF  THIS SOURCE CODE AND/OR RELATED INFORMATION DOES NOT CONVEY OR IMPLY ANY RIGHTS
* TO REPRODUCE, DISCLOSE OR DISTRIBUTE ITS CONTENTS, OR TO MANUFACTURE, USE, OR SELL ANYTHING THAT IT  MAY DESCRIBE, IN WHOLE OR IN PART.
*/

namespace OpenWrapSDK.Common
{
    /// <summary>
    /// Contains log specific constants
    /// </summary>
    internal static class POBLogStrings
    {
        internal static readonly string DestroyMethodLog = "Calling destroy method.";

        #region Android's POBDataProviderClient class logs
        internal static readonly string AddSegmentFailedLog = "Unable to add segment as either data provider or segment is null.";
        internal static readonly string RemoveSegmentFailedLog = "Unable to remove segment as received segment is null.";
        #endregion

        #region Android's POBUserInfoClient class logs
        internal static readonly string AddDataProviderFailedLog = "Unable to add DataProvider as either UserInfo or received DataProvider or its client is null.";
        internal static readonly string RemoveDataProviderFailedLog = "Unable to remove DataProvider as received DataProvider is null.";
        #endregion

        #region Android OpenWrapSDKClient class logs
        internal static readonly string SetUserInfoObjectFailedLog = "Failed to get native object of UserInfo.";
        internal static readonly string SetUserInfoFailedLog = "Unable to set user info as either userInfo or its client is null";
        internal static readonly string AddExternalUserIdFailedLog = "Unable to add externalUserId as required parameter is null.";
        internal static readonly string RemoveExternalUserIdsFailedLog = "Unable to remove external user Id as source string is null.";
        internal static readonly string SetApplicationInfoStoreURLLog = "Unable to set application info as StoreURL is not provided.";
        internal static readonly string SetApplicationInfoPaidLog = "Unable to set application info as value of Paid is unknown.";
        #endregion

        #region POBAndroidUtils class logs
        internal static readonly string ConvertToPOBErrorEventArgsLog = "Unable to convert to ErrorEventArgs as received native error instance is null.";
        internal static readonly string GetPOBRewardEventArgsLog = "Unable to convert to RewardEventArgs as received native RewardClient instance is null.";
        internal static readonly string ConvertGenderToJavaObjectLog = "Invalid gender value received";
        internal static readonly string GetPOBGenderLog = "Unable to convert to respective Gender as received native enum is null.";
        internal static readonly string ConvertListToJavaListLog = "Empty List received for conversion into Java list.";
        internal static readonly string ConvertJavaMapToDictionaryLog = "Unable to convert to dictionary as received native map is null.";
        internal static readonly string ConvertDictionaryToJavaJSONObjectLog = "Unable to convert to native JSONObject as received dictionary is null.";
        internal static readonly string ConvertJavaJSONObjectToDictionaryLog = "Unable to convert to c# dictionary as received native JSONObject is null.";
        internal static readonly string ConvertToPOBAdSizeLog = "Unable to convert to native AdSize as passed AdSize is null.";
        #endregion

        #region POBUserInfo class logs
        internal static readonly string AddDataProviderSuccessLog = "Adding dataProvider for {0}.";
        internal static readonly string RemoveDataProviderSuccessLog = "Removing dataProvider for {0}.";
        internal static readonly string RemoveDataProviderFailedWithNameLog = "Unable to remove {0} dataProvider as dataProviders list is empty.";
        internal static readonly string RemoveAllDataProvidersLog = "Removing all dataProviders.";
        #endregion

        #region Client class logs
        internal static readonly string ClientDestroyLog = "Calling native API: destroy.";
        internal static readonly string ClientLoadAdLog = "Calling native API: loadAd.";
        internal static readonly string ClientPauseAutoRefreshLog = "Calling native API: pauseAutoRefresh.";
        internal static readonly string ClientResumeAutoRefreshLog = "Calling native API: resumeAutoRefresh.";
        internal static readonly string ClientForceRefreshLog = "Calling native API: forceRefresh.";
        internal static readonly string ClientIsReadyLog = "Calling native API: isReady.";
        internal static readonly string ClientShowAdLog = "Calling native API: showAd.";
        internal static readonly string ClientHasWonLog = "Calling native API: hasWon";
        internal static readonly string ClientIsExpiredLog = "Calling native API: isExpired";
        internal static readonly string ClientToStringLog = "Calling native API: toString";
        internal static readonly string ClientAddSegmentLog = "Calling native API : addSegment";
        internal static readonly string ClientRemoveSegmentLog = "Calling native API: removeSegment";
        internal static readonly string ClientRemoveAllSegmentsLog = "Calling native API: removeAllSegments";
        internal static readonly string ClientRemoveDataProviderLog = "Calling native API: removeDataProvider";
        internal static readonly string ClientRemoveAllDataProvidersLog = "Calling native API: removeAllDataProviders";
        internal static readonly string ClientAddDataProviderLog = "Calling native API: addDataProvider";
        internal static readonly string ClientEqualsLog = "Calling native API: equals";
        #endregion

        #region Public POBOpenWrapSDK class
        internal static readonly string AddExternalUserIdPartnerIdLog = "External User Id with duplicate partner Id not allowed";
        internal static readonly string AddExternalUserIdUserIdLog = "Failed to add external user Id, as one/more mandatory parameters are missing.";
        #endregion

        internal static readonly string GetBannerRequest = "Calling POBUGetBannerRequest method";
        internal static readonly string GetBannerImpression = "Calling POBUGetBannerImpression method";
        internal static readonly string DestroyBanner = "Calling POBUDestroyBanner method";

        internal static readonly string GetBannerBid = "Calling POBUGetBannerBid method";

        internal static readonly string GetBannerCreativeSize = "Calling POBUGetBannerCreativeSize method";
        internal static readonly string SetBannerPosition = "Calling POBUSetBannerPosition method";
        internal static readonly string SetBannerCustomPostion = "Calling POBUSetBannerCustomPostion method";
        internal static readonly string PauseAutoRefresh = "Calling POBUPauseAutoRefresh method";
        internal static readonly string ResumeAutoRefresh = "Calling POBUResumeAutoRefresh method";
        internal static readonly string ForceRefresh = "Calling POBUForceRefresh method";
        internal static readonly string CreateBanner = "Calling POBUCreateBanner method";
        internal static readonly string SetBannerViewCallbacks = "Calling POBUSetBannerViewCallbacks method";
        internal static readonly string GetInterstitialRequest = "Calling POBUGetInterstitialRequest method";
        internal static readonly string GetInterstitialImpression = "Calling POBUGetInterstitialImpression method";

        internal static readonly string LoadInterstitial = "Calling POBULoadInterstitial method";
        internal static readonly string ShowInterstitial = "Calling POBUShowInterstitial method";
        internal static readonly string DestroyInterstitial = "Calling POBUDestroyInterstitial method";
        internal static readonly string CreateInterstitial = "Calling POBUCreateInterstitial method";
        internal static readonly string GetRewardedAdRequest = "Calling POBUGetRewardedAdRequest method";
        internal static readonly string GetRewardedAdImpression = "Calling POBUGetRewardedAdImpression method";
        internal static readonly string LoadRewardedAd = "Calling POBULoadRewardedAd method";
        internal static readonly string ShowRewardedAd = "Calling POBUShowRewardedAd method";
        internal static readonly string SetRewardedSkipAlertInfo = "Calling POBUSetRewardedSkipAlertInfo method";
        internal static readonly string DestroyRewardedAd = "Calling POBUDestroyRewardedAd method";
        internal static readonly string RewardedAdSetClient = "Calling POBURewardedAdSetClient method";

        internal static readonly string BidGetTargetingKeys = "Calling POBUBidGetTargetingKeys method";
        internal static readonly string BidGetTargetingValues = "Calling POBUBidGetTargetingValues method";
        internal static readonly string Targetting = "Received valid key and value, so adding it to targetting dictionary.";
        internal static readonly string BidIsExpired = "Calling POBUBidIsExpired method";

        internal static readonly string AddSegment = "Calling POBUAddSegment method";
        internal static readonly string RemoveSegment = "Calling POBURemoveSegment method";
        internal static readonly string RemoveAllSegments = "Calling POBURemoveAllSegments method";
        internal static readonly string DestroyDataProvider = "Calling POBUDestroyDataProvider method";
        internal static readonly string SetDataProviderExtension = "Calling POBUSetDataProviderExtension method";

        internal static readonly string DestroyExternalUserId = "Calling POBUDestroyExternalUserId method";
        internal static readonly string SetExtension = "Calling POBUSetExtension method";
        internal static readonly string SetCustomParams = "Calling POBUSetCustomParams method";

        internal static readonly string DestroySegment = "Calling POBUDestroySegment method";
        internal static readonly string UserInfoAddDataProvider = "Calling POBUUserInfoAddDataProvider method";
        internal static readonly string UserInfoRemoveAllDataProviders = "Calling POBUUserInfoRemoveAllDataProviders method";
        internal static readonly string UserInfoRemoveDataProvider = "Calling POBUUserInfoRemoveDataProvider method";
        internal static readonly string DestroyUserInfo = "Calling POBUDestroyUserInfo method";


        internal static readonly string AddExternalUserId = "Calling POBUAddExternalUserId method";
        internal static readonly string RemoveAllExternalUserIds = "Calling POBURemoveAllExternalUserIds method";
        internal static readonly string RemoveExternalUserIdsWithSource = "Calling POBURemoveExternalUserIdsWithSource method";

        internal static readonly string InvalidSourceAndId = "Please provide valid source and identifier";
        internal static readonly string InvalidName = "Please provide valid name to create POBDataProvider instance";
        internal static readonly string InvalidNameAndId = "Please provide valid name and indentifier to create POBDataProvider instance";

        internal static readonly string DuplicateSegmentNotAllowed = "Segments with duplicate id not allowed";
        internal static readonly string InvalidSegment = "Please provide valid segment instance";
    }
}
#endif