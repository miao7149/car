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

#import "POBUAdsCacheManager.h"

@interface POBUAdsCacheManager()
@property(nonatomic, strong) NSMutableDictionary *adsCacheDict;
@end

@implementation POBUAdsCacheManager

+ (POBUAdsCacheManager *)manager {
    static POBUAdsCacheManager *sharedManager = nil;
    static dispatch_once_t pred;
    
    dispatch_once(&pred, ^{
        sharedManager = [[POBUAdsCacheManager alloc] init];
        sharedManager.adsCacheDict = [[NSMutableDictionary alloc] init];
    });
    
    return sharedManager;
}

- (void)addObject:(id)object {
    [self.adsCacheDict setObject:object forKey:[self keyForObject:object]];
}

- (void)removeObject:(id)object {
    [self.adsCacheDict removeObjectForKey:[self keyForObject:object]];
}

- (id)objectForKey:(NSString *)key {
    return [self.adsCacheDict objectForKey:key];
}

- (NSString *)keyForObject:(id)object {
    return [NSString stringWithFormat:@"%lu", (unsigned long)((NSObject*)object).hash];
}

- (void)dealloc {
    self.adsCacheDict = nil;
}

@end
