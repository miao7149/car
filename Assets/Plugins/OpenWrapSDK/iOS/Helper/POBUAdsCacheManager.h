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

/**
 Class to store all the ad objects in a cache, so that when they are passed to Unity, they will not get deallocated before using them.
 */
@interface POBUAdsCacheManager : NSObject

/**
 @abstract Method to get the shared manager.
 @return Shared instance of POBUAdsCacheManager
 */
+ (POBUAdsCacheManager *)manager;

/**
 @abstract Method to add object in the ads cache.
 @param object Ad instance to be saved in the cache
 */
- (void)addObject:(id)object;

/**
 @abstract Method to remove the object from ads cache.
 @param object  Ad instance to be removed from the cache
 */
- (void)removeObject:(id)object;

/**
 @abstract Getter for an ad object from the cache for given key.
 @param key Key used for saving the object in the cache.
 @return Ad object for the given key
 */
- (id)objectForKey:(NSString *)key;

/**
 @abstract Method to get the key specific to the object passed
 @param object Ad object for which key is to be generated
 @return Key string
 */
- (NSString *)keyForObject:(id)object;

@end
