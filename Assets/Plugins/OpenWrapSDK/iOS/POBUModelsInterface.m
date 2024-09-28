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
#import "POBUUtil.h"
#import "POBUTypes.h"
#import <OpenWrapSDK/POBBid.h>
#import <OpenWrapSDK/POBRequest.h>
#import <OpenWrapSDK/POBDataProvider.h>
#import "POBUAdsCacheManager.h"

#pragma mark - POBUserInfo

void POBUSetUserInfoBirthYear(POBUUserInfoRef userInfo, int birthYear) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.birthYear = [NSNumber numberWithInt:birthYear];
}

void POBUSetUserInfoGender(POBUUserInfoRef userInfo, int gender) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    if (gender <= POBGenderFemale) {
        internalUserInfo.gender = gender;
    }
}

void POBUSetUserInfoMetro(POBUUserInfoRef userInfo, const char *metro) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.metro = [POBUUtil POBUNSStringFromCharsArray:metro];
}

void POBUSetUserInfoZip(POBUUserInfoRef userInfo, const char *zip) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.zip = [POBUUtil POBUNSStringFromCharsArray:zip];
}

void POBUSetUserInfoCity(POBUUserInfoRef userInfo, const char *city) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.city = [POBUUtil POBUNSStringFromCharsArray:city];
}

void POBUSetUserInfoRegion(POBUUserInfoRef userInfo, const char *region) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.region = [POBUUtil POBUNSStringFromCharsArray:region];
}

void POBUSetUserInfoKeywords(POBUUserInfoRef userInfo, const char *keywords) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    internalUserInfo.keywords = [POBUUtil POBUNSStringFromCharsArray:keywords];
}

void POBUUserInfoAddDataProvider(POBUUserInfoRef userInfo, POBUTypeDataProviderRef dataProvider) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    [internalUserInfo addDataProvider:internalDataProvider];
    
    // Remove the data provider from plugin's cache. It will be retained by UserInfo, as it is added in userInfo above.
    [[POBUAdsCacheManager manager] removeObject:internalDataProvider];
}

void POBUUserInfoRemoveDataProvider(POBUUserInfoRef userInfo, POBUTypeDataProviderRef dataProvider) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    
    // Remove the data provider from the userinfo
    [internalUserInfo removeDataProviderWithName:internalDataProvider.name];
}

void POBUUserInfoRemoveAllDataProviders(POBUUserInfoRef userInfo) {
    POBUserInfo *internalUserInfo = (__bridge POBUserInfo *)userInfo;
    [internalUserInfo removeAllDataProviders];
}

#pragma mark - POBBid parameters getter/setter APIs

// Get bid id
const char* POBUBidGetBidId(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.bidId];
}

// Get impression id from bid
const char* POBUBidGetImpressionId(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.impressionId];
}

// Get price from bid
double POBUBidGetPrice(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.price.doubleValue;
}

// Get gross price from bid
double POBUBidGetGrossPrice(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.grossPrice.doubleValue;
}

// Get width from bid
int POBUBidGetWidth(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.size.width;
}

// Get height from bid
int POBUBidGetHeight(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.size.height;
}

// Get status from bid
NSInteger POBUBidGetStatus(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.status.integerValue;
}

// Get creative id from bid
const char* POBUBidGetCreativeId(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.creativeId];
}

// Get creative tag from bid
const char* POBUBidGetCreativeTag(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.creativeTag];
}

// Get creative type from bid
const char* POBUBidGetCreativeType(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.creativeType];
}

// Get partner name from bid
const char* POBUBidGetPartner(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.partner];
}

// Get deal id from bid
const char* POBUBidGetDealId(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.dealId];
}

// Get if bid has won OpenWrap auction
bool POBUBidGetHasWon(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.hasWon;
}

// Set if bid has won the client side auction
void POBUBidSetHasWon(POBUTypeBidRef bid, bool hasWon) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    internalBid.hasWon = hasWon;
}

// Get the PubMatic partner id
const char* POBUBidGetPubMaticPartnerId(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCharArrayFromNSString:internalBid.partner];
}

/// Get targeting information
// Get list of targeting keys
const char ** POBUBidGetTargetingKeys(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return [POBUUtil POBUCStringsArrayFrom:internalBid.targetingInfo.allKeys];
}

// Get list of targeting values
const char ** POBUBidGetTargetingValues(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    NSArray *keys = internalBid.targetingInfo.allKeys;
    NSMutableArray *values = [NSMutableArray new];
    
    // The targeting value can be of any data type, so convert it into string, as expected by Unity API.
    for (NSString *key in keys) {
        id value = [internalBid.targetingInfo valueForKey:key];
        if ([value isKindOfClass:[NSString class]]) {
            [values addObject:value];
        } else if ([value isKindOfClass:[NSNumber class]]) {
            [values addObject:((NSNumber *)value).stringValue];
        }
        // For other type of values e.g. dictionary, array, objects, they are not yet expected
        // as part of targeting, so ignoring them.
    }
    return [POBUUtil POBUCStringsArrayFrom:values];
}

// Get count of targeting key-value pairs
NSInteger POBUBidGetTargetingCount(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.targetingInfo.count;
}

// Get if the bid is expired
bool POBUBidIsExpired(POBUTypeBidRef bid) {
    POBBid *internalBid = (__bridge POBBid *)bid;
    return internalBid.isExpired;
}

#pragma mark - POBRequest parameters getter/setter APIs

void POBUSetVersionId(POBUTypeAdRequestRef request, NSInteger versionId) {
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.versionId = [NSNumber numberWithInteger:versionId];
}

NSInteger POBUGetVersionId(POBUTypeAdRequestRef request) {
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.versionId.integerValue;
}

void POBUSetNetworkTimeout(POBUTypeAdRequestRef request, NSInteger timeout){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.networkTimeout = timeout;
}

NSInteger POBUGetNetworkTimeout(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.networkTimeout;
}

void POBUSetAdServerUrl(POBUTypeAdRequestRef request, const char * url){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.adServerURL = [POBUUtil POBUNSStringFromCharsArray:url];
}

const char* POBUGetAdServerUrl(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return [POBUUtil POBUCharArrayFromNSString:internalRequest.adServerURL];
}

void POBUSetTestModeEnabled(POBUTypeAdRequestRef request, BOOL enable){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.testModeEnabled = enable;
}

BOOL POBUIsTestModeEnabled(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.isTestModeEnabled;
}

void POBUSetDebugStateEnabled(POBUTypeAdRequestRef request, BOOL enable){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.debug = enable;
}

BOOL POBUIsDebugStateEnabled(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.debug;
}

void POBUSetBidSummaryEnabled(POBUTypeAdRequestRef request, BOOL enable){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    internalRequest.bidSummaryEnabled = enable;
}

BOOL POBUIsBidSummaryEnabled(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.bidSummaryEnabled;
}

NSString * POBUGetPubId(POBUTypeAdRequestRef request) {
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.publisherId;
}

NSInteger POBUGetProfileId(POBUTypeAdRequestRef request){
    POBRequest *internalRequest = (__bridge POBRequest *)request;
    return internalRequest.profileId;
}

#pragma mark - POBImpression parameters getter/setter APIs

void POBUSetZoneId(POBUTypeAdImpressionRef impression, const char * zoneId){
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    internalImpression.pmZoneId = [POBUUtil POBUNSStringFromCharsArray:zoneId];
}

const char* POBUGetZoneId(POBUTypeAdImpressionRef impression){
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    return [POBUUtil POBUCharArrayFromNSString:internalImpression.pmZoneId];
}

void POBUSetTestCreativeId(POBUTypeAdImpressionRef impression, const char * creativeId) {
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    internalImpression.testCreativeId = [POBUUtil POBUNSStringFromCharsArray:creativeId];
}

const char* POBUGetTestCreativeId(POBUTypeAdImpressionRef impression){
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    return [POBUUtil POBUCharArrayFromNSString:internalImpression.testCreativeId];
}

// Set custom parameters
void POBUSetCustomParams(POBUTypeAdImpressionRef impression, const char * key, const char ** valuesList, int valuesCount){
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    
    // Get previously set customParams
    NSMutableDictionary *customParams = internalImpression.customParams.mutableCopy;
    
    // Get the list of values
    NSArray *values = [POBUUtil POBUNSArrayFromCStringsList:valuesList count:valuesCount];
    if (customParams == nil) {
        // Create dictionary if it is nil.
        customParams = [[NSMutableDictionary alloc] init];
    }
    // set the list in custom params dict
    [customParams setObject:values forKey:[POBUUtil POBUNSStringFromCharsArray:key]];
    internalImpression.customParams = [NSDictionary dictionaryWithDictionary:customParams];
}

void POBUSetAdPosition(POBUTypeAdImpressionRef impression, NSInteger position){
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    if (position <= POBAdPositionFullscreen) {
        internalImpression.adPosition = position;
    }
}

NSInteger POBUGetAdPosition(POBUTypeAdImpressionRef impression) {
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    return internalImpression.adPosition;
}

const char* POBUGetAdUnitId(POBUTypeAdImpressionRef impression) {
    POBImpression *internalImpression = (__bridge POBImpression *)impression;
    return [POBUUtil POBUCharArrayFromNSString:internalImpression.adUnitId];
}

#pragma mark - Segment Id and Data provider
#pragma mark Segment Id

// Create POBSegment instance
POBUTypeSegmentRef POBUCreateSegment(const char* identifier, const char* name) {
    POBSegment *segment = nil;
    if (name == NULL) {
        segment = [[POBSegment alloc] initWithId:[POBUUtil POBUNSStringFromCharsArray:identifier]];
    } else {
        segment = [[POBSegment alloc] initWithId:[POBUUtil POBUNSStringFromCharsArray:identifier]
                                         andName:[POBUUtil POBUNSStringFromCharsArray:name]];
    }
    // Add the segment instance into cache, to retain it.
    [[POBUAdsCacheManager manager] addObject:segment];
    return (__bridge POBUTypeSegmentRef)segment;
}

// Remove the segment from cache
void POBUDestroySegment(POBUTypeSegmentRef segment) {
    POBSegment *internalSegment = (__bridge POBSegment *)segment;
    [[POBUAdsCacheManager manager] removeObject:internalSegment];
}

// Set segment value
void POBUSetSegmentValue(POBUTypeSegmentRef segment, const char* value) {
    POBSegment *internalSegment = (__bridge POBSegment *)segment;
    internalSegment.value = [POBUUtil POBUNSStringFromCharsArray:value];
}

#pragma mark Data Provider

// Create POBDataProvider instance
POBUTypeDataProviderRef POBUCreateDataProvider(const char* name, const char* identifier) {
    POBDataProvider *dataProvider = nil;
    if (identifier == NULL) {
        dataProvider = [[POBDataProvider alloc] initWithName:[POBUUtil POBUNSStringFromCharsArray:name]];
    } else {
        dataProvider = [[POBDataProvider alloc] initWithName:[POBUUtil POBUNSStringFromCharsArray:name]
                                                       andId:[POBUUtil POBUNSStringFromCharsArray:identifier]];
    }
    // Add the data provider instance into cache, to retain it, until it is added into user info
    [[POBUAdsCacheManager manager] addObject:dataProvider];
    return (__bridge POBUTypeDataProviderRef)dataProvider;
}

// Add segment
void POBUAddSegment(POBUTypeDataProviderRef dataProvider, POBUTypeSegmentRef segment) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    POBSegment *internalSegment = (__bridge POBSegment *)segment;
    [internalDataProvider addSegment:internalSegment];
    
    // Remove the segment from plugin's cache. It will be reatined by data provider, as it is added in data provider above.
    [[POBUAdsCacheManager manager] removeObject:internalSegment];
}

// Remove the segment of given identifier
void POBURemoveSegment(POBUTypeDataProviderRef dataProvider, POBUTypeSegmentRef segment) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    POBSegment *internalSegment = (__bridge POBSegment *)segment;
    
    // remove it from the data provider
    [internalDataProvider removeSegmentForId:internalSegment.identifier];
}

// Remove all segments from data provider
void POBURemoveAllSegments(POBUTypeDataProviderRef dataProvider) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    [internalDataProvider removeAllSegments];
}

// Get data provider seg tax
void POBUSetDataProviderSegTax(POBUTypeDataProviderRef dataProvider, int segTax) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    internalDataProvider.segTax = segTax;
}

// Set data provider extension, with keys list and values list
void POBUSetDataProviderExtension(POBUTypeDataProviderRef dataProvider, const char ** keysList, const char ** valuesList, int dictCount) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    
    // Get the list of keys
    NSArray *keys = [POBUUtil POBUNSArrayFromCStringsList:keysList count:dictCount];
    
    // Get the list of values
    NSArray *values = [POBUUtil POBUNSArrayFromCStringsList:valuesList count:dictCount];
    
    // Get previously set extension
    NSMutableDictionary *extension = internalDataProvider.ext.mutableCopy;
    if (extension == nil) {
        // Create dictionary if it is nil.
        extension = [[NSMutableDictionary alloc] init];
    }
    
    for (int index = 0; index < dictCount; index++) {
        // Set each key, value in the dictionary
        [extension setObject:[values objectAtIndex:index]
                      forKey:[keys objectAtIndex:index]];
    }
    
    internalDataProvider.ext = [NSDictionary dictionaryWithDictionary:extension];
}

// Destroy Data Provider
void POBUDestroyDataProvider(POBUTypeDataProviderRef dataProvider) {
    POBDataProvider *internalDataProvider = (__bridge POBDataProvider *)dataProvider;
    [[POBUAdsCacheManager manager] removeObject:internalDataProvider];
}
