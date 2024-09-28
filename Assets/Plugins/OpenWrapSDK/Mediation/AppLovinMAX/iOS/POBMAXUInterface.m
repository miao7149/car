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

// IntPtr of local extras values objects
typedef const void *POBObjectTypeRef;

// Dictionary to cache the local extras values objects
static NSMutableDictionary *POBLocalExtrasValuesCache = nil;

#pragma mark - Helper methods

// Method to get NSString from characters array
NSString * POBNSStringFromCharsArray(const char *str) {
    if (str == NULL) {
        return nil;
    }
    return [NSString stringWithCString:str encoding:NSUTF8StringEncoding];
}

// Method to get the key for an object, against which, it will be saved in local extras values dictionary.
// The key is - <max_adunit>_<object_string_representation>
NSString * POBLocalExtrasKey(const char *adunit, id object) {
    return [NSString stringWithFormat:@"%@_%lu", POBNSStringFromCharsArray(adunit), (unsigned long)((NSObject*)object).hash];
}

// Save the object in local extras values cache dictionary, to retain the values in the memory.
void POBSaveObject(const char * adUnit, id object) {
    if (!POBLocalExtrasValuesCache) {
        POBLocalExtrasValuesCache = [[NSMutableDictionary alloc] init];
    }
    NSString *keyString = POBLocalExtrasKey(adUnit, object);
    [POBLocalExtrasValuesCache setObject:object forKey:keyString];
}

#pragma mark - Plugin methods

// Method to convert the string into IntPtr as required by MAX unity plugin
POBObjectTypeRef POBCreateStringIntPtr(const char * adUnit, const char * value) {
    NSString *valueString = POBNSStringFromCharsArray(value);
    POBSaveObject(adUnit, valueString);
    return (__bridge POBObjectTypeRef)valueString;
}

// Method to convert the integer into IntPtr as required by MAX unity plugin
POBObjectTypeRef POBCreateIntegerIntPtr(const char * adUnit, int value) {
    NSNumber *valueNumber = [NSNumber numberWithInt:value];
    POBSaveObject(adUnit, valueNumber);
    return (__bridge POBObjectTypeRef)valueNumber;
}

// Method to convert the bool into IntPtr as required by MAX unity plugin
POBObjectTypeRef POBCreateBoolIntPtr(const char * adUnit, BOOL value) {
    NSNumber *valueNumber = [NSNumber numberWithBool:value];
    POBSaveObject(adUnit, valueNumber);
    return (__bridge POBObjectTypeRef)valueNumber;
}

// Clear the saved objects of local extras values from cache
void POBClearLocalExtrasForAdUnit(const char * adUnit) {
    NSString *adUnitStr = POBNSStringFromCharsArray(adUnit);
    NSArray *allKeys = POBLocalExtrasValuesCache.allKeys;
    
    // Search all the keys, and remove the objects for the passed ad unit
    for (NSString *key in allKeys) {
        if ([key containsString:adUnitStr]) {
            [POBLocalExtrasValuesCache removeObjectForKey:key];
        }
    }
}
