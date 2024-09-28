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

#import "POBUUtil.h"
#import "UnityAppController.h"
#import <OpenWrapSDK/POBDataTypes.h>

@implementation POBUUtil

// Returns NSString from c type characters array
+ (NSString *)POBUNSStringFromCharsArray:(const char *)bytes {
    return bytes ? @(bytes) : nil;
}

// Returns a copied string chars array
+ (const char *)POBUCharArrayFromNSString:(NSString *)string {
    if (!string) {
        return NULL;
    }
    char *newString = strdup(string.UTF8String);
    return newString;
}

// Returns list of C-type strings
+ (const char **)POBUCStringsArrayFrom:(NSArray *)array {
    if (!array.count) {
        return nil;
    }
    
    // Create a 2D characters arrays
    const char **stringsArray = calloc(array.count, sizeof(char *));
    for (int i = 0; i < array.count; i++) {
        // Convert each sting as a characters array and add it to the resulting array
        stringsArray[i] = [self POBUCharArrayFromNSString:[array objectAtIndex:i]];
    }
    return stringsArray;
}

+ (NSArray *)POBUNSArrayFromCStringsList:(const char **)stringsList count:(int)count {
    NSMutableArray *stringsArray = [[NSMutableArray alloc] init];
    
    // Iterate through the list
    for (int i = 0; i < count; i++) {
        // Convert each c-string into NSString
        NSString *objcString = [self POBUNSStringFromCharsArray:stringsList[i]];
        [stringsArray addObject:objcString];
    }
    
    return [NSArray arrayWithArray:stringsArray];
}

// Get the correct ORTB position for rect in UnityView
+ (POBAdPosition)rtbAdPositionForRect:(CGRect)rect {
    // Set default position as unknown
    POBAdPosition rtbAdPosition = POBAdPositionUnKnown;
    
    // Current view frame
    CGRect viewFrame = UnityGetGLView().frame;
    
    // Get possible visible banner area on screen
    CGRect intersection = CGRectIntersection(rect, viewFrame);
    
    if (intersection.size.width > 0 && intersection.size.height > 0) {
        // Banner position is inside the screen view
        // Calculate the vertical center on the screen
        float viewCenterY = viewFrame.size.height / 2.0f;
        
        // If y-coordinate is in top half, set the position as "Header"
        // If y-coordinate is in bottom half, set the position as "Footer"
        rtbAdPosition = (rect.origin.y <= viewCenterY) ? POBAdPositionHeader : POBAdPositionFooter;
    }
    return rtbAdPosition;
}

@end
