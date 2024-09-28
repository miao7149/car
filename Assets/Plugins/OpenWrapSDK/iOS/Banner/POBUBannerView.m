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

#import "POBUBannerView.h"
#import "POBUUtil.h"
#import "POBUUIUtil.h"

@interface POBUBannerView() <POBBannerViewDelegate>
@property (nonatomic, strong) POBBannerView *bannerView;
@property (nonatomic, assign) POBUTypeAdClientRef *bannerClient;
@property (nonatomic, strong) NSArray<POBAdSize *> *adSizes;
@end

@implementation POBUBannerView

// Inititalize POBUBannerView. Create POBBannerView internally
- (instancetype)initWithBannerClient:(POBUTypeAdClientRef *)bannerClient
                         publisherId:(NSString *)publisherId
                           profileId:(NSInteger)profileId
                            adUnitId:(NSString *)adUnitId
                             adSizes:(NSArray<POBAdSize *> *)adSizes {
    self = [super init];
    if (self) {
        _bannerClient = bannerClient;
        _adSizes = adSizes;
        _bannerView = [[POBBannerView alloc] initWithPublisherId:publisherId
                                                       profileId:[NSNumber numberWithInteger:profileId]
                                                        adUnitId:adUnitId
                                                         adSizes:adSizes];
        _bannerView.delegate = self;
    }
    return self;
}

- (void)dealloc {
    [self cleanup];
}

// Load bannerview
- (void)loadAd {
    [_bannerView loadAd];
}

- (void)cleanup {
    if (self.bannerView) {
        [self.bannerView removeFromSuperview];
        self.bannerView = nil;
    }
}

// Pausing the autorefresh
- (void)pauseAutoRefresh {
    [self.bannerView pauseAutoRefresh];
}

// Resuming the already paused autorefresh
- (void)resumeAutoRefresh {
    [self.bannerView resumeAutoRefresh];
}

// Refresh forcefully
- (BOOL)forceRefresh {
    return [self.bannerView forceRefresh];
}

// Set the banner position on view
- (void)setPosition:(POBUBannerPosition)position {
    _position = position;
    
    if (position == POBUBannerPositionTopLeft ||
        position == POBUBannerPositionTopRight ||
        position == POBUBannerPositionTopCenter ||
        position == POBUBannerPositionCenter) {
        // Map top and center position to "Header"
        self.bannerView.impression.adPosition = POBAdPositionHeader;
        
    } else if (position == POBUBannerPositionBottomLeft ||
               position == POBUBannerPositionBottomRight ||
               position == POBUBannerPositionBottomCenter) {
        // Map bottom position to "Footer"
        self.bannerView.impression.adPosition = POBAdPositionFooter;
    }
}

// Set the banner custom position on view
- (void)setCustomPosition:(CGPoint)customPosition {
    _customPosition = customPosition;
    
    // Single ad size is supported for unity banner, so first POBAdSize is taken from the array
    POBAdSize *adSize = self.adSizes.firstObject;
    
    // Get the correct ORTB ad position from the custom position
    self.bannerView.impression.adPosition = [POBUUtil rtbAdPositionForRect:CGRectMake(customPosition.x, customPosition.y, adSize.width, adSize.height)];
}

#pragma mark - POBInterstitialDelegate

// Methof to get the viewcontroller for banner presentation.
- (nonnull UIViewController *)bannerViewPresentationController { 
    return UnityGetGLViewController();
}

// Baner load success callback
- (void)bannerViewDidReceiveAd:(POBBannerView *)bannerView {
    self.bannerView = bannerView;
    
    if (!self.bannerView.superview) {
        // The view is not already attached to the screen as per the position specified by publisher.
        // So, attach banner to screen
        [self addBannerToView];
    }
    
    if (self.didLoadAdCallback) {
        self.didLoadAdCallback(self.bannerClient);
    }
}

// Banner load failed callback
- (void)bannerView:(POBBannerView *)bannerView didFailToReceiveAdWithError:(NSError *)error {
    if (self.didFailToLoadAdCallback) {
        self.didFailToLoadAdCallback(self.bannerClient, error.code, error.localizedDescription.UTF8String);
    }
}

// Banner ad clicked callback
- (void)bannerViewDidClickAd:(POBBannerView *)bannerView {
    if (self.didClickAdCallback) {
        self.didClickAdCallback(self.bannerClient);
    }
}

// Method to notify view presentation callback due to banner click
- (void)bannerViewWillPresentModal:(POBBannerView *)bannerView {
    if (self.didPresentCallback) {
        self.didPresentCallback(self.bannerClient);
    }
}

// Method to notify view dismissal callback which is presented due to banner click
- (void)bannerViewDidDismissModal:(POBBannerView *)bannerView {
    if (self.didDismissCallback) {
        self.didDismissCallback(self.bannerClient);
    }
}

// Method to notify app leaving callback due to banner click
- (void)bannerViewWillLeaveApplication:(POBBannerView *)bannerView {
    if (self.willLeaveAppCallback) {
        self.willLeaveAppCallback(self.bannerClient);
    }
}

#pragma mark - Private methods

// Attach banner to the view
- (void)addBannerToView {
    // Adjust banner frame
    CGSize adSize = self.bannerView.creativeSize.cgSize;
    CGFloat x = (self.position == POBUBannerPositionCustom) ? self.customPosition.x : self.bannerView.frame.origin.x;
    CGFloat y = (self.position == POBUBannerPositionCustom) ? self.customPosition.y : self.bannerView.frame.origin.y;
    
    // Attach banner to unity view
    [POBUUIUtil attachBannerView:self.bannerView
                       withFrame:CGRectMake(x, y, adSize.width, adSize.height)
                     andPosition:self.position];
}

@end
