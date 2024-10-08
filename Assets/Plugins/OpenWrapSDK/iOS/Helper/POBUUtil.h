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
#import <OpenWrapSDK/POBImpression.h>

/**
 @abstract Util class to accomodate all the helper methods for Unity plugin
 */
@interface POBUUtil : NSObject

/**
 @abstract Method to get a NSString from c type characters array
 @param bytes Characters array
 @return NSString from characters array
 */
+ (NSString *)POBUNSStringFromCharsArray:(const char*)bytes;

/**
 @abstract Method to get a copy of a string as chars array
 @param string String to be converted into characters array
 @return Characters array
 */
+ (const char*)POBUCharArrayFromNSString:(NSString *)string;

/**
 @abstract Returns list of C-type strings
 @params List of NSString
 @return List of C-type chracter arrays
 */
+ (const char **)POBUCStringsArrayFrom:(NSArray<NSString *> *)array;

/**
 @abstract Method to convert C-style strings list into NSArray
 @param stringsList List of C-type strings
 @param count Length of stringsList array
 @return NSArray of NSString
 */
+ (NSArray *)POBUNSArrayFromCStringsList:(const char **)stringsList count:(int)count;

/**
 @abstract Get the correct ORTB position for rect in UnityView
 @param rect Frame of the view, whose position is requested
 @return Position of the view in UnityView as POBAdPosition
 @seealso POBAdPosition
 */
+ (POBAdPosition)rtbAdPositionForRect:(CGRect)rect;

@end
