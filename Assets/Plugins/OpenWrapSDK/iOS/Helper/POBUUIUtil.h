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

#import "POBUConstants.h"

/**
 @abstract Util class for UIView related methods
 */
@interface POBUUIUtil : NSObject

/**
 @abstract Get the correct top anchor, considering the safe area if required.
 @param view UIView whose top anchor is requested
 @return NSLayoutYAxisAnchor top anchor
 */
+ (NSLayoutYAxisAnchor *)safeTopAnchorOfView:(UIView *)view;

/**
 @abstract Get the correct bottom anchor, considering the safe area if required.
 @param view UIView whose bottom anchor is requested
 @return NSLayoutYAxisAnchor bottom anchor
 */
+ (NSLayoutYAxisAnchor *)safeBottomAnchorOfView:(UIView *)view;

/**
 @abstract Get the correct leading anchor, considering the safe area if required.
 @param view UIView whose leading anchor is requested
 @return NSLayoutXAxisAnchor top anchor
 */
+ (NSLayoutXAxisAnchor *)safeLeadingAnchorOfView:(UIView *)view;

/**
 @abstract Get the correct trailing anchor, considering the safe area if required.
 @param view UIView whose trailing anchor is requested
 @return NSLayoutXAxisAnchor top anchor
 */
+ (NSLayoutXAxisAnchor *)safeTrailingAnchorOfView:(UIView *)view;

/**
 @abstract Attach banner view to the unity screen
 @param view BannerView
 @param frame Frame of banner view
 @param position Banner position
 @seealso POBUBannerPosition
 */
+ (void)attachBannerView:(UIView *)view
               withFrame:(CGRect)frame
             andPosition:(POBUBannerPosition)position;

@end
