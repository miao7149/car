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


#import "POBURewardedAd.h"
#import "UnityAppController.h"
#import "POBUUtil.h"

@interface POBURewardedAd()<POBRewardedAdDelegate>
@property (nonatomic, strong) POBRewardedAd *rewardedAd;
@end

@implementation POBURewardedAd

+ (POBURewardedAd*)rewardedWithPublisherId:(NSString *)publisherId
                                 profileId:(NSInteger)profileId
                                  adUnitId:(NSString *)adUnitId {
    POBRewardedAd *rewardedAd = [POBRewardedAd rewardedAdWithPublisherId:publisherId
                                                 profileId:[NSNumber numberWithInteger:profileId]
                                                  adUnitId:adUnitId];
    if (rewardedAd) {
        return [[POBURewardedAd alloc] initWithRewardedAd:rewardedAd];
    }
    return nil;
}

// Inititalize POBURewardedAd with iOS POBRewardedAd object
- (instancetype)initWithRewardedAd:(POBRewardedAd *)rewardedAd {
    self = [super init];
    if (self) {
        _rewardedAd = rewardedAd;
        _rewardedAd.delegate = self;
    }
    return self;
}

- (void)dealloc {
    _rewardedAd = nil;
}

// Load rewarded ad
- (void)loadAd {
    [_rewardedAd loadAd];
}

// Show rewarded ad
- (void)showAd {
    [_rewardedAd showFromViewController:UnityGetGLViewController()];
}

// Set custom skip alert details
- (void)setSkipAlertInfoWithTitle:(NSString *)title
                          message:(NSString *)message
                 closeButtonTitle:(NSString *)closeTitle
                resumeButtonTitle:(NSString *)resumeTitle {
    [_rewardedAd setSkipAlertInfoWithTitle:title
                                   message:message
                          closeButtonTitle:closeTitle
                         resumeButtonTitle:resumeTitle];
}

#pragma mark - POBRewardedAdDelegate

// RewardedAd load success callback
- (void)rewardedAdDidReceiveAd:(POBRewardedAd *)rewardedAd {
    if (self.didLoadAdCallback) {
        self.didLoadAdCallback(self.rewardedAdClient);
    }
}

// rewardedAd load failed callback
- (void)rewardedAd:(POBRewardedAd *)rewardedAd didFailToReceiveAdWithError:(NSError *)error {
    if (self.didFailToLoadAdCallback) {
        self.didFailToLoadAdCallback(self.rewardedAdClient, error.code, error.localizedDescription.UTF8String);
    }
}

// rewardedAd show failed callback
- (void)rewardedAd:(POBRewardedAd *)rewardedAd didFailToShowAdWithError:(NSError *)error {
    if (self.didFailToShowAdCallback) {
        self.didFailToShowAdCallback(self.rewardedAdClient, error.code, error.localizedDescription.UTF8String);
    }
}

// Method to notify that the rewarded ad has been presented
- (void)rewardedAdDidPresentAd:(POBRewardedAd *)rewardedAd {
    if (self.didPresentCallback) {
        self.didPresentCallback(self.rewardedAdClient);
    }
}

// Method to notify that the ad has been animated off the screen
- (void)rewardedAdDidDismissAd:(POBRewardedAd *)rewardedAd {
    if (self.didDismissCallback) {
        self.didDismissCallback(self.rewardedAdClient);
    }
}

// Method to notify app leaving callback due to rewardedAd click
- (void)rewardedAdWillLeaveApplication:(POBRewardedAd *)rewardedAd {
    if (self.willLeaveAppCallback) {
        self.willLeaveAppCallback(self.rewardedAdClient);
    }
}

// rewardedAd ad clicked callback
- (void)rewardedAdDidClickAd:(POBRewardedAd *)rewardedAd {
    if (self.didClickAdCallback) {
        self.didClickAdCallback(self.rewardedAdClient);
    }
}

// rewardedAd ad expired callback
- (void)rewardedAdDidExpireAd:(POBRewardedAd *)rewardedAd {
    if (self.didExpireAdCallback) {
        self.didExpireAdCallback(self.rewardedAdClient);
    }
}

// rewardedAd ad shouldReward callback
-(void)rewardedAd:(POBRewardedAd *)rewardedAd shouldReward:(POBReward *)reward {
    if (self.shouldRewardAdCallback) {
        self.shouldRewardAdCallback(self.rewardedAdClient, reward.amount.integerValue, reward.currencyType.UTF8String);
    }
}


@end
