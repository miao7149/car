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

#import "POBUInterstitial.h"
#import "UnityAppController.h"
#import "POBUUtil.h"

@interface POBUInterstitial()<POBInterstitialDelegate, POBInterstitialVideoDelegate>
@property (nonatomic, strong) POBInterstitial *interstitial;
@property (nonatomic, assign) POBUTypeAdClientRef *interstitialClient;
@end

@implementation POBUInterstitial

// Inititalize POBUInterstitial. Create POBInterstitial internally
- (instancetype)initWithInterstitialClient:(POBUTypeAdClientRef *)interstitialClient
                               publisherId:(NSString *)publisherId
                                 profileId:(NSInteger)profileId
                                  adUnitId:(NSString *)adUnitId {
    self = [super init];
    if (self) {
        _interstitialClient = interstitialClient;
        _interstitial = [[POBInterstitial alloc] initWithPublisherId:publisherId
                                                           profileId:[NSNumber numberWithInteger:profileId]
                                                            adUnitId:adUnitId];
        _interstitial.delegate = self;
        _interstitial.videoDelegate = self;
    }
    return self;
}

- (void)dealloc {
    _interstitial = nil;
}

// Load interstitial ad
- (void)loadAd {
    [_interstitial loadAd];
}

// Show interstitial ad
- (void)showAd {
    [_interstitial showFromViewController:UnityGetGLViewController()];
}

#pragma mark - POBInterstitialDelegate

// Interstitial load success callback
- (void)interstitialDidReceiveAd:(POBInterstitial *)interstitial {
    if (self.didLoadAdCallback) {
        self.didLoadAdCallback(self.interstitialClient);
    }
}

// Interstitial load failed callback
- (void)interstitial:(POBInterstitial *)interstitial didFailToReceiveAdWithError:(NSError *)error {
    if (self.didFailToLoadAdCallback) {
        self.didFailToLoadAdCallback(self.interstitialClient, error.code, error.localizedDescription.UTF8String);
    }
}

// Interstitial show failed callback
- (void)interstitial:(POBInterstitial *)interstitial didFailToShowAdWithError:(NSError *)error {
    if (self.didFailToShowAdCallback) {
        self.didFailToShowAdCallback(self.interstitialClient, error.code, error.localizedDescription.UTF8String);
    }
}

// Method to notify view presentation callback due to interstitial click
- (void)interstitialDidPresentAd:(POBInterstitial *)interstitial {
    if (self.didPresentCallback) {
        self.didPresentCallback(self.interstitialClient);
    }
}

// Method to notify view dismissal callback which is presented due to interstitial click
- (void)interstitialDidDismissAd:(POBInterstitial *)interstitial {
    if (self.didDismissCallback) {
        self.didDismissCallback(self.interstitialClient);
    }
}

// Method to notify app leaving callback due to interstitial click
- (void)interstitialWillLeaveApplication:(POBInterstitial *)interstitial {
    if (self.willLeaveAppCallback) {
        self.willLeaveAppCallback(self.interstitialClient);
    }
}

// Interstitial ad clicked callback
- (void)interstitialDidClickAd:(POBInterstitial *)interstitial {
    if (self.didClickAdCallback) {
        self.didClickAdCallback(self.interstitialClient);
    }
}

// Interstitial ad expired callback
- (void)interstitialDidExpireAd:(POBInterstitial *)interstitial {
    if (self.didExpireAdCallback) {
        self.didExpireAdCallback(self.interstitialClient);
    }
}

#pragma mark - POBInterstitialVideoDelegate

// Video interstitial finished playing callback
- (void)interstitialDidFinishVideoPlayback:(POBInterstitial *)interstitial {
    if (self.didFinishVideoCallback) {
        self.didFinishVideoCallback(self.interstitialClient);
    }
}

@end
