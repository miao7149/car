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
#import "POBUTypes.h"
#import <OpenWrapSDK/POBRewardedAd.h>

/**
 @abstract Wrapper class over POBRewardedAd, responsible for creation of POBRewardedAd, listeneing to all it's callbacks and pass them on to Unity.
 */
@interface POBURewardedAd : NSObject

/**
 @abstract Instance of POBRewardedAd
 */
@property (nonatomic, readonly) POBRewardedAd *rewardedAd;

/**
 @abstract Reference of Unity's rewardedAd client
 */
@property (nonatomic, assign) POBUTypeAdClientRef *rewardedAdClient;

#pragma mark - RewardedAd callbacks
/**
 @abstract Reference of ad loaded callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didLoadAdCallback;

/**
 @abstract Reference of ad failed to load callback from Unity
 */
@property (nonatomic, assign) POBUAdFailureCallback didFailToLoadAdCallback;

/**
 @abstract Reference of ad failed to show callback from Unity
 */
@property (nonatomic, assign) POBUAdFailureCallback didFailToShowAdCallback;

/**
 @abstract Reference of ad presented a modal callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didPresentCallback;

/**
 @abstract Reference of ad dimissed the presented modal callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didDismissCallback;

/**
 @abstract Reference of app leave  callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback willLeaveAppCallback;

/**
 @abstract Reference of ad clicked callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didClickAdCallback;

/**
 @abstract Reference of ad expired callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didExpireAdCallback;

/**
 @abstract Reference of ad should reward callback from Unity
 */
@property (nonatomic, assign) POBUAdRewardCallback shouldRewardAdCallback;

/**
 @abstract Convenience initializer for POBURewardedAd class
 @param publisherId Publisher Id for OpenWrap
 @param profileId Profile Id for OpenWrap
 @param adUnitId Ad unit Id for OpenWrap
 @return Intance of POBURewardedAd
 */
+ (POBURewardedAd*)rewardedWithPublisherId:(NSString *)publisherId
                                profileId:(NSInteger)profileId
                                 adUnitId:(NSString *)adUnitId;

/**
 @abstract Method to load ad.
 */
- (void)loadAd;

/**
 @abstract Method to show ad.
 */
- (void)showAd;

/*!
 @abstract Sets title, message & button texts to be used while showing the skip alert.
 @discussion If any of the parameters is empty the SDK discards all the values & shows a default skip alert.
 @param title alert title
 @param message alert message
 @param closeTitle title of the close button
 @param resumeTitle title of the resume button
 */
- (void)setSkipAlertInfoWithTitle:(NSString*)title
                          message:(NSString*)message
                 closeButtonTitle:(NSString*)closeTitle
                resumeButtonTitle:(NSString*)resumeTitle;
@end

