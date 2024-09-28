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

#import "POBUUIUtil.h"

@implementation POBUUIUtil

// Get the top anchor of a view, considering the safe area.
+ (NSLayoutYAxisAnchor *)safeTopAnchorOfView:(UIView *)view {
    if (@available(iOS 11.0, *)) {
        return view.safeAreaLayoutGuide.topAnchor;
    }
    return view.topAnchor;
}

// Get the bottom anchor of a view, considering the safe area.
+ (NSLayoutYAxisAnchor *)safeBottomAnchorOfView:(UIView *)view {
    if (@available(iOS 11.0, *)) {
        return view.safeAreaLayoutGuide.bottomAnchor;
    }
    return view.bottomAnchor;
}

// Get the leading anchor of a view, considering the safe area.
+ (NSLayoutXAxisAnchor *)safeLeadingAnchorOfView:(UIView *)view {
    if (@available(iOS 11.0, *)) {
        return view.safeAreaLayoutGuide.leadingAnchor;
    }
    return view.leadingAnchor;
}

// Get the trailing anchor of a view, considering the safe area.
+ (NSLayoutXAxisAnchor *)safeTrailingAnchorOfView:(UIView *)view {
    if (@available(iOS 11.0, *)) {
        return view.safeAreaLayoutGuide.trailingAnchor;
    }
    return view.trailingAnchor;
}

// Attach the banner to unity view
+ (void)attachBannerView:(UIView *)view withFrame:(CGRect)frame andPosition:(POBUBannerPosition)position {
    // Get unity view
    UIView *unityView = UnityGetGLView();
    [unityView addSubview:view];
    [view setTranslatesAutoresizingMaskIntoConstraints:NO];
    
    // Get width and height constraints of the view
    NSLayoutConstraint *widthConstraint = [view.widthAnchor constraintEqualToConstant:frame.size.width];
    NSLayoutConstraint *heightConstraint = [view.heightAnchor constraintEqualToConstant:frame.size.height];
        
    NSMutableArray *constraints = [NSMutableArray arrayWithObjects: widthConstraint, heightConstraint, nil];
    
    if (position == POBUBannerPositionCustom) {
        NSLayoutConstraint *top = [view.topAnchor constraintEqualToAnchor:[self safeTopAnchorOfView:unityView]
                                                                 constant:frame.origin.y];
        NSLayoutConstraint *leading = [view.leadingAnchor constraintEqualToAnchor:[self safeLeadingAnchorOfView:unityView]
                                                                         constant:frame.origin.x];
        [constraints addObject:top];
        [constraints addObject:leading];
        
    } else if (position == POBUBannerPositionCenter) {
        NSLayoutConstraint *centerX = [view.centerXAnchor constraintEqualToAnchor:unityView.centerXAnchor];
        NSLayoutConstraint *centerY = [view.centerYAnchor constraintEqualToAnchor:unityView.centerYAnchor];
        [constraints addObject:centerX];
        [constraints addObject:centerY];
        
    } else if (position == POBUBannerPositionTopLeft) {
        NSLayoutConstraint *top = [view.topAnchor constraintEqualToAnchor:[self safeTopAnchorOfView:unityView]];
        NSLayoutConstraint *leading = [view.leadingAnchor constraintEqualToAnchor:[self safeLeadingAnchorOfView:unityView]];
        [constraints addObject:top];
        [constraints addObject:leading];
            
    } else if (position == POBUBannerPositionTopRight) {
        NSLayoutConstraint *top = [view.topAnchor constraintEqualToAnchor:[self safeTopAnchorOfView:unityView]];
        NSLayoutConstraint *trailing = [view.trailingAnchor constraintEqualToAnchor:[self safeTrailingAnchorOfView:unityView]];
        [constraints addObject:top];
        [constraints addObject:trailing];
            
    } else if (position == POBUBannerPositionTopCenter) {
        NSLayoutConstraint *top = [view.topAnchor constraintEqualToAnchor:[self safeTopAnchorOfView:unityView]];
        NSLayoutConstraint *centerX = [view.centerXAnchor constraintEqualToAnchor:unityView.centerXAnchor];
        [constraints addObject:top];
        [constraints addObject:centerX];
        
    } else if (position == POBUBannerPositionBottomLeft) {
        NSLayoutConstraint *bottom = [view.bottomAnchor constraintEqualToAnchor:[self safeBottomAnchorOfView:unityView]];
        NSLayoutConstraint *leading = [view.leadingAnchor constraintEqualToAnchor:[self safeLeadingAnchorOfView:unityView]];
        [constraints addObject:bottom];
        [constraints addObject:leading];
            
    } else if (position == POBUBannerPositionBottomRight) {
        NSLayoutConstraint *bottom = [view.bottomAnchor constraintEqualToAnchor:[self safeBottomAnchorOfView:unityView]];
        NSLayoutConstraint *trailing = [view.trailingAnchor constraintEqualToAnchor:[self safeTrailingAnchorOfView:unityView]];
        [constraints addObject:bottom];
        [constraints addObject:trailing];
            
    } else {
        // position is POBUBannerPositionBottomCenter
        NSLayoutConstraint *bottom = [view.bottomAnchor constraintEqualToAnchor:[self safeBottomAnchorOfView:unityView]];
        NSLayoutConstraint *centerX = [view.centerXAnchor constraintEqualToAnchor:unityView.centerXAnchor];
        [constraints addObject:bottom];
        [constraints addObject:centerX];
    }
     
    [NSLayoutConstraint activateConstraints:constraints];
}

@end
