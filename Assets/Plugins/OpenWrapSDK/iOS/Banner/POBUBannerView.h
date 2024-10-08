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
#import <OpenWrapSDK/POBBannerView.h>
#import "POBUTypes.h"
#import "POBUConstants.h"

/**
 @abstract Wrapper class over POBBannerView, responsible for creation of POBBannerView, listeneing to all it's callbacks
 and pass them on to Unity.
 */
@interface POBUBannerView : NSObject

/**
 @abstract Instance of POBBannerView
 */
@property (nonatomic, readonly) POBBannerView *bannerView;

/**
 @abstract Reference of Unity's banner client
 */
@property (nonatomic, readonly) POBUTypeAdClientRef *bannerClient;

/**
 @abstract Ad postion of bannerview
 @seealso POBUBannerPosition
 */
@property (nonatomic, assign) POBUBannerPosition position;

/**
 @abstract Custom ad position
 */
@property (nonatomic, assign) CGPoint customPosition;

#pragma mark - Banner callbacks
/**
 @abstract Reference of ad loaded callback from Unity
 */
@property (nonatomic, assign) POBUAdCallback didLoadAdCallback;

/**
 @abstract Reference of ad failed to load callback from Unity
 */
@property (nonatomic, assign) POBUAdFailureCallback didFailToLoadAdCallback;

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
 @abstract Initializer for POBUBannerView class
 @param bannerClient Reference of Unity's banner client
 @param publisherId Publisher Id for OpenWrap
 @param profileId Profile Id for OpenWrap
 @param adUnitId Ad unit Id for OpenWrap
 @param adSizes List of POBAdSizes.
 @return Instance of POBUBannerView
 */
- (instancetype)initWithBannerClient:(POBUTypeAdClientRef *)bannerClient
                         publisherId:(NSString *)publisherId
                           profileId:(NSInteger)profileId
                            adUnitId:(NSString *)adUnitId
                             adSizes:(NSArray<POBAdSize *> *)adSizes;

/**
 @abstract Method to load ad.
 */
- (void)loadAd;

/**
 @abstract Method to cleanup banner view and also to remove it from it's superview.
 */
- (void)cleanup;

/**
 @abstract Pauses the auto refresh, By default, banner refreshes automatically as per
 configured refresh interval on openwrap portal.
 */
- (void)pauseAutoRefresh;

/**
 @abstract Resumes the autorefresh as per configured refresh interval on openwrap portal, call this method only if you have previously paused autorefresh using `pauseAutoRefresh`.
 */
- (void)resumeAutoRefresh;

/**
 @abstract Cancels existing ad requests and initiates new ad request
 @warning It may skip force refresh if ad creative is being loaded, user interacting with ad (Opening Internal browser or expanding ad) or waiting response from ad server SDK if applicable.
 @result Status YES/NO, about force refresh, as described it can skip in few cases by returning 'NO'
 */
- (BOOL)forceRefresh;

@end
