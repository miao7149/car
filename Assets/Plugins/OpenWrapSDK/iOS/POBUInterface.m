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

#import <Foundation/Foundation.h>
#import "POBUUtil.h"
#import "POBUInterstitial.h"
#import "POBURewardedAd.h"
#import "POBUBannerView.h"
#import "POBUConstants.h"
#import "POBUAdsCacheManager.h"
#import <OpenWrapSDK/OpenWrapSDK.h>

#pragma mark - OpenWrap SDK Global settings

// Set OpenWrap SDK version
const char* POBUGetOpenWrapSDKVersion() {
    return [POBUUtil POBUCharArrayFromNSString:[OpenWrapSDK version]];
}

// Set Log level
void POBUSetLogLevel(NSInteger logLevel) {
    if (logLevel <= POBSDKLogLevelAll) {
        [OpenWrapSDK setLogLevel:logLevel];
    }
}

// Set allow location access
void POBUAllowLocationAccess(BOOL enable) {
    [OpenWrapSDK allowLocationAccess:enable];
}

// Set use internal browser
void POBUUseInternalBrowser(BOOL use) {
    [OpenWrapSDK useInternalBrowser:use];
}

// Set location
void POBUSetLocation(double longitude, double latitude, NSInteger locSource) {
    if (locSource <= POBLocSourceUserProvided) {
        CLLocation *location = [[CLLocation alloc] initWithLatitude:latitude longitude:longitude];
        [OpenWrapSDK setLocation:location source:locSource];
    }
}

// Set COPPA enabled
void POBUSetCOPPAEnabled(BOOL enable) {
    [OpenWrapSDK setCoppaEnabled:enable];
}

// Set SSL enabled
void POBUSetSSLEnabled(BOOL enable) {
    [OpenWrapSDK setSSLEnabled:enable];
}

// Set allow advertising Id
void POBUAllowAdvertisingId(BOOL allow) {
    [OpenWrapSDK allowAdvertisingId:allow];
}

// Set allow AVAudioSessionAccess
void POBUAllowAVAudioSessionAccess(BOOL allow) {
    [OpenWrapSDK allowAVAudioSessionAccess:allow];
}

// Set POBApplicationInfo
void POBUSetApplicationInfo(POBApplicationInfoInternal *appInfoInternal) {
    POBApplicationInfo *applicationInfo = [[POBApplicationInfo alloc] init];
    applicationInfo.domain = [POBUUtil POBUNSStringFromCharsArray:appInfoInternal->domain];
    applicationInfo.categories = [POBUUtil POBUNSStringFromCharsArray:appInfoInternal->categories];
    applicationInfo.keywords = [POBUUtil POBUNSStringFromCharsArray:appInfoInternal->keywords];
    applicationInfo.paid = appInfoInternal->paid;
    applicationInfo.storeURL = [NSURL URLWithString:[POBUUtil POBUNSStringFromCharsArray:appInfoInternal->storeUrl]];
    [OpenWrapSDK setApplicationInfo:applicationInfo];
}

// Get application info
void POBUGetApplicationInfoInto(struct POBApplicationInfoInternal* appInfoInternal) {
    POBApplicationInfo *appInfo = OpenWrapSDK.applicationInfo;
    if (appInfo) {
        appInfoInternal->domain = [POBUUtil POBUCharArrayFromNSString:appInfo.domain];
        appInfoInternal->categories = [POBUUtil POBUCharArrayFromNSString:appInfo.categories];
        appInfoInternal->keywords = [POBUUtil POBUCharArrayFromNSString:appInfo.keywords];
        appInfoInternal->paid = (int)appInfo.paid;
        appInfoInternal->storeUrl = [POBUUtil POBUCharArrayFromNSString:appInfo.storeURL.absoluteString];
    }
}

// Get user info
POBUUserInfoRef POBUGetUserInfo() {
    POBUserInfo *userInfo = [OpenWrapSDK userInfo];
    if (!userInfo) {
        userInfo = [[POBUserInfo alloc] init];
        // Add it into the cache to retain it, until it is set into OpenWrapSDK class.
        [[POBUAdsCacheManager manager] addObject:userInfo];
    }
    return (__bridge POBUUserInfoRef)userInfo;
}

// Set user info
void POBUSetUserInfo(POBUUserInfoRef userInfo) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    [OpenWrapSDK setUserInfo:internalUserInfo];
    // Remove the user info from cache, as it is now owned and retained by OpenWrapSDK class
    [[POBUAdsCacheManager manager] removeObject:internalUserInfo];
}

// destroy user info
void POBUDestroyUserInfo(POBUUserInfoRef userInfo) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    [[POBUAdsCacheManager manager] removeObject:internalUserInfo];
}

#pragma mark - Interstitial
// Create Interstitial with publisher Id, profile id, ad unit id
POBUTypeAdRef POBUCreateInterstitial(POBUTypeAdClientRef *interstitialClient,
                                     const char *publisherId,
                                     NSInteger profileId,
                                     const char *adUnitId) {
    
    NSString *pubIdStr = [POBUUtil POBUNSStringFromCharsArray:publisherId];
    NSString *adUnitIdStr = [POBUUtil POBUNSStringFromCharsArray:adUnitId];
    
    POBUInterstitial *interstitial = [[POBUInterstitial alloc]
                                      initWithInterstitialClient:interstitialClient
                                      publisherId:pubIdStr
                                      profileId:profileId
                                      adUnitId:adUnitIdStr];
    [[POBUAdsCacheManager manager] addObject:interstitial];
    return (__bridge POBUTypeAdRef)interstitial;
}

// Set Interstitial callbacks
void POBUSetInterstitialCallbacks(POBUTypeAdRef interstitial,
                                  POBUAdCallback didLoadAdCallback,
                                  POBUAdFailureCallback didFailToLoadAdCallback,
                                  POBUAdFailureCallback didFailToShowAdCallback,
                                  POBUAdCallback didPresentCallback,
                                  POBUAdCallback didDismissCallback,
                                  POBUAdCallback willLeaveAppCallback,
                                  POBUAdCallback didClickAdCallback,
                                  POBUAdCallback didExpireAdCallback) {
    
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    internalInterstitial.didLoadAdCallback = didLoadAdCallback;
    internalInterstitial.didFailToLoadAdCallback = didFailToLoadAdCallback;
    internalInterstitial.didFailToShowAdCallback = didFailToShowAdCallback;
    internalInterstitial.didPresentCallback = didPresentCallback;
    internalInterstitial.didDismissCallback = didDismissCallback;
    internalInterstitial.willLeaveAppCallback = willLeaveAppCallback;
    internalInterstitial.didClickAdCallback = didClickAdCallback;
    internalInterstitial.didExpireAdCallback = didExpireAdCallback;
}

// Set Video Interstitial callbacks
void POBUSetVideoInterstitialCallbacks(POBUTypeAdRef interstitial,
                                       POBUAdCallback didFinishVideoCallback) {
    
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    internalInterstitial.didFinishVideoCallback = didFinishVideoCallback;
}

// Load interstitial
void POBULoadInterstitial(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    [internalInterstitial loadAd];
}

// Show interstitial
void POBUShowInterstitial(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    [internalInterstitial showAd];
}

// Destroy interstitial
void POBUDestroyInterstitial(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    [[POBUAdsCacheManager manager] removeObject:internalInterstitial];
}

// Check if interstitial is ready to show
BOOL POBUIsInterstitialReady(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    return internalInterstitial.interstitial.isReady;
}

// Get bid from interstitial instance
POBUTypeBidRef POBUGetInterstitialBid(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    return (__bridge POBUTypeBidRef)internalInterstitial.interstitial.bid;
}

// Get request from interstitial instance
POBUTypeAdRequestRef POBUGetInterstitialRequest(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    return (__bridge POBUTypeAdRequestRef)internalInterstitial.interstitial.request;
}

// Get impression from interstitial instance
POBUTypeAdImpressionRef POBUGetInterstitialImpression(POBUTypeAdRef interstitial) {
    POBUInterstitial *internalInterstitial = (__bridge POBUInterstitial *)interstitial;
    return (__bridge POBUTypeAdImpressionRef)internalInterstitial.interstitial.impression;
}

#pragma mark - Banner
// Create banner with publisher Id, profile id, ad unit id
POBUTypeAdRef POBUCreateBanner(POBUTypeAdClientRef *bannerClient,
                               const char *publisherId,
                               NSInteger profileId,
                               const char *adUnitId,
                               NSInteger width,
                               NSInteger height) {
    
    NSString *pubIdStr = [POBUUtil POBUNSStringFromCharsArray:publisherId];
    NSString *adUnitIdStr = [POBUUtil POBUNSStringFromCharsArray:adUnitId];
    
    POBUBannerView *bannerView = [[POBUBannerView alloc] initWithBannerClient:bannerClient
                                                                  publisherId:pubIdStr
                                                                    profileId:profileId
                                                                     adUnitId:adUnitIdStr
                                                                      adSizes:@[ POBAdSizeMake(width, height) ]];
    
    [[POBUAdsCacheManager manager] addObject:bannerView];
    return (__bridge POBUTypeAdRef)bannerView;
}

// Set the standard banner position. Please refer POBUBannerPosition for more details.
void POBUSetBannerPosition(POBUTypeAdRef bannerView, int position) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    internalBannerView.position = position;
}

// Set the custom banner position, mentioned as per the coordinates.
void POBUSetBannerCustomPostion(POBUTypeAdRef bannerView, float x, float y) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    internalBannerView.position = POBUBannerPositionCustom;
    internalBannerView.customPosition = CGPointMake(x, y);
}

// Set Interstitial callbacks
void POBUSetBannerViewCallbacks(POBUTypeAdRef bannerView,
                                POBUAdCallback didLoadAdCallback,
                                POBUAdFailureCallback didFailToLoadAdCallback,
                                POBUAdCallback didPresentCallback,
                                POBUAdCallback didDismissCallback,
                                POBUAdCallback willLeaveAppCallback,
                                POBUAdCallback didClickAdCallback) {
    
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    internalBannerView.didLoadAdCallback = didLoadAdCallback;
    internalBannerView.didFailToLoadAdCallback = didFailToLoadAdCallback;
    internalBannerView.didPresentCallback = didPresentCallback;
    internalBannerView.didDismissCallback = didDismissCallback;
    internalBannerView.willLeaveAppCallback = willLeaveAppCallback;
    internalBannerView.didClickAdCallback = didClickAdCallback;
}

// Load banner
void POBULoadBanner(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    [internalBannerView loadAd];
}

// Get banner creative size
const char * POBUGetBannerCreativeSize(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    POBAdSize *size = internalBannerView.bannerView.creativeSize;
    return [POBUUtil POBUCharArrayFromNSString:[NSString stringWithFormat:@"%dx%d", (int)size.width, (int)size.height]];
}

// Destroy interstitial
void POBUDestroyBanner(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    [internalBannerView cleanup];
    [[POBUAdsCacheManager manager] removeObject:internalBannerView];
}

// Pause auto refresh
void POBUPauseAutoRefresh(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    [internalBannerView pauseAutoRefresh];
}

// Resume auto refresh
void POBUResumeAutoRefresh(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    [internalBannerView resumeAutoRefresh];
}

// Force refresh
BOOL POBUForceRefresh(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    return [internalBannerView forceRefresh];
}

// Get bid from banner instance
POBUTypeBidRef POBUGetBannerBid(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    return (__bridge POBUTypeBidRef)internalBannerView.bannerView.bid;
}

// Get request from banner instance
POBUTypeAdRequestRef POBUGetBannerRequest(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    return (__bridge POBUTypeAdRequestRef)internalBannerView.bannerView.request;
}

// Get impression from banner instance
POBUTypeAdImpressionRef POBUGetBannerImpression(POBUTypeAdRef bannerView) {
    POBUBannerView *internalBannerView = (__bridge POBUBannerView *)bannerView;
    return (__bridge POBUTypeAdImpressionRef)internalBannerView.bannerView.impression;
}

#pragma mark - POBExternalUserId

POBUTypeExternalUserId POBUCreateExternalUserId(const char *source, const char *userId) {
    NSString *sourceStr = [POBUUtil POBUNSStringFromCharsArray:source];
    NSString *userIdStr = [POBUUtil POBUNSStringFromCharsArray:userId];
    POBExternalUserId *user = [[POBExternalUserId alloc] initWithSource:sourceStr andId:userIdStr];
    
    [[POBUAdsCacheManager manager] addObject:user];
    return (__bridge POBUTypeExternalUserId)user;
}

void POBUAddExternalUserId(POBUTypeExternalUserId externalUserId) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    [OpenWrapSDK addExternalUserId:extUserId];
    [[POBUAdsCacheManager manager] removeObject:extUserId];
}

void POBURemoveAllExternalUserIds() {
    [OpenWrapSDK removeAllExternalUserIds];
}

void POBURemoveExternalUserIdsWithSource(const char *source) {
    [OpenWrapSDK removeExternalUserIdsWithSource:[POBUUtil POBUNSStringFromCharsArray:source]];
}

void POBUDestroyExternalUserId(POBUTypeExternalUserId externalUserId) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    [[POBUAdsCacheManager manager] removeObject:extUserId];
}

const char * POBUGetSource(POBUTypeExternalUserId externalUserId) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    return [POBUUtil POBUCharArrayFromNSString:[extUserId source]];
}

const char * POBUGetExternalUserId(POBUTypeExternalUserId externalUserId) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    return [POBUUtil POBUCharArrayFromNSString:[extUserId externalUserId]];
}

void POBUSetAType(POBUTypeExternalUserId externalUserId, int atype) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    [extUserId setAtype:atype];
}

int POBUGetAType(POBUTypeExternalUserId externalUserId) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    return [extUserId atype];
}

void POBUSetExtension(POBUTypeExternalUserId externalUserId, const char ** keysList, const char ** valuesList, int dictCount) {
    POBExternalUserId *extUserId = (__bridge POBExternalUserId *)externalUserId;
    // Get the list of keys
     NSArray *keys = [POBUUtil POBUNSArrayFromCStringsList:keysList count:dictCount];

     // Get the list of values
     NSArray *values = [POBUUtil POBUNSArrayFromCStringsList:valuesList count:dictCount];
    
    // Get previously set extension
         NSMutableDictionary *extension = extUserId.extension.mutableCopy;
    if (extension == nil) {
        // Create dictionary if it is nil.
        extension = [[NSMutableDictionary alloc] init];
    }

    for (int index = 0; index < dictCount; index++) {
       // Set each key, value in the dictionary
       [extension setObject:[values objectAtIndex:index]
                     forKey:[keys objectAtIndex:index]];
    }
    [extUserId setExtension:[NSDictionary dictionaryWithDictionary:extension]];
}

#pragma mark - Rewarded Ad
POBUTypeAdRef POBUCreateRewardedAd(const char *publisherId,
                                   NSInteger profileId,
                                   const char *adUnitId) {
    
    NSString *pubIdStr = [POBUUtil POBUNSStringFromCharsArray:publisherId];
    NSString *adUnitIdStr = [POBUUtil POBUNSStringFromCharsArray:adUnitId];
    
    POBURewardedAd *rewardedAd = [POBURewardedAd rewardedWithPublisherId:pubIdStr
                                                               profileId:profileId
                                                                adUnitId:adUnitIdStr];
    if (rewardedAd) {
        [[POBUAdsCacheManager manager] addObject:rewardedAd];
        return (__bridge POBUTypeAdRef)rewardedAd;
    }
    return nil;
}

// set rewarded client ptr
void POBURewardedAdSetClient(POBUTypeAdRef rewardedAd, POBUTypeAdClientRef *rewardedAdClient) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    internalRewardedAd.rewardedAdClient = rewardedAdClient;
}

// Set rewarded ad callbacks
void POBUSetRewardedAdCallbacks(POBUTypeAdRef rewardedAd,
                                POBUAdCallback didLoadAdCallback,
                                POBUAdFailureCallback didFailToLoadAdCallback,
                                POBUAdFailureCallback didFailToShowAdCallback,
                                POBUAdCallback didPresentCallback,
                                POBUAdCallback didDismissCallback,
                                POBUAdCallback willLeaveAppCallback,
                                POBUAdCallback didClickAdCallback,
                                POBUAdCallback didExpireAdCallback,
                                POBUAdRewardCallback shouldRewardAdCallback) {
    
    POBURewardedAd *internalRewardedAd  = (__bridge POBURewardedAd *)rewardedAd;
    internalRewardedAd.didLoadAdCallback = didLoadAdCallback;
    internalRewardedAd.didFailToLoadAdCallback = didFailToLoadAdCallback;
    internalRewardedAd.didFailToShowAdCallback = didFailToShowAdCallback;
    internalRewardedAd.didPresentCallback = didPresentCallback;
    internalRewardedAd.didDismissCallback = didDismissCallback;
    internalRewardedAd.willLeaveAppCallback = willLeaveAppCallback;
    internalRewardedAd.didClickAdCallback = didClickAdCallback;
    internalRewardedAd.didExpireAdCallback = didExpireAdCallback;
    internalRewardedAd.shouldRewardAdCallback = shouldRewardAdCallback;
}

// Load rewarded ad
void POBULoadRewardedAd(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    [internalRewardedAd loadAd];
}

// Show RewardedAd
void POBUShowRewardedAd(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    [internalRewardedAd showAd];
}

// Set Skip alert info - title, message, close/resume buttons titles
void POBUSetRewardedSkipAlertInfo(const char *title, const char *message, const char *closeTitle, const char *resumeTitle, POBUTypeAdRef rewardedAd) {
    NSString *titleStr = [POBUUtil POBUNSStringFromCharsArray:title];
    NSString *msgStr = [POBUUtil POBUNSStringFromCharsArray:message];
    NSString *closeTitleStr = [POBUUtil POBUNSStringFromCharsArray:closeTitle];
    NSString *resumeTitleStr = [POBUUtil POBUNSStringFromCharsArray:resumeTitle];
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    [internalRewardedAd setSkipAlertInfoWithTitle:titleStr
                                          message:msgStr
                                 closeButtonTitle:closeTitleStr
                                resumeButtonTitle:resumeTitleStr];
}

// Destroy RewardedAd
void POBUDestroyRewardedAd(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    [[POBUAdsCacheManager manager] removeObject:internalRewardedAd];
}

// Check if RewardedAd is ready to show
BOOL POBUIsRewardedAdReady(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    return internalRewardedAd.rewardedAd.isReady;
}

// Get bid from RewardedAd instance
POBUTypeBidRef POBUGetRewardedBid(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    return (__bridge POBUTypeBidRef)internalRewardedAd.rewardedAd.bid;
}

// Get request from RewardedAd instance
POBUTypeAdRequestRef POBUGetRewardedAdRequest(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    return (__bridge POBUTypeAdRequestRef)internalRewardedAd.rewardedAd.request;
}

// Get impression from RewardedAd instance
POBUTypeAdImpressionRef POBUGetRewardedAdImpression(POBUTypeAdRef rewardedAd) {
    POBURewardedAd *internalRewardedAd = (__bridge POBURewardedAd *)rewardedAd;
    return (__bridge POBUTypeAdImpressionRef)internalRewardedAd.rewardedAd.impression;
}
